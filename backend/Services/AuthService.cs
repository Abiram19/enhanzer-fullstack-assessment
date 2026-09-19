using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using EnhanzerAssessment.Api.DTOs;
using Microsoft.Extensions.Logging;

namespace EnhanzerAssessment.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocationService _locationService;
        private readonly ILogger<AuthService> _logger;
        private const string ExternalApiUrl = "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";

        public AuthService(HttpClient httpClient, ILocationService locationService, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _locationService = locationService;
            _logger = logger;
        }

        public async Task<ClientLoginResponseDto> AuthenticateAsync(LoginRequestDto request)
        {
            var payload = new ExternalLoginRequest
            {
                API_Action = "GetLoginData",
                Device_Id = "D001",
                Sync_Time = "",
                Company_Code = request.Email,
                API_Body = new ExternalLoginBody
                {
                    Username = request.Email,
                    Pw = request.Password
                }
            };

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                var response = await _httpClient.PostAsync(ExternalApiUrl, content, cts.Token);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"External API failed with status code: {response.StatusCode}");
                    return new ClientLoginResponseDto { Success = false, StatusCode = (int)response.StatusCode, Message = $"External API error: {response.StatusCode}" };
                }

                var responseString = await response.Content.ReadAsStringAsync();
                
                // Parse the response using JsonDocument to safely navigate potential nulls/arrays/objects
                using var jsonDoc = JsonDocument.Parse(responseString);
                var root = jsonDoc.RootElement;
                
                int statusCode = root.TryGetProperty("Status_Code", out var statusCodeElement) ? statusCodeElement.GetInt32() : 0;
                string message = root.TryGetProperty("Message", out var messageElement) ? messageElement.GetString() ?? "" : "";
                
                // Check if the response contains "Invalid Login Details"
                bool isInvalidDetails = false;
                if (root.TryGetProperty("Response_Body", out var respBodyCheck) && respBodyCheck.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in respBodyCheck.EnumerateArray())
                    {
                        if (item.TryGetProperty("Doc_Msg", out var docMsgElem) && docMsgElem.GetString() == "Invalid Login Details")
                        {
                            isInvalidDetails = true;
                            break;
                        }
                    }
                }
                
                if (isInvalidDetails)
                {
                    return new ClientLoginResponseDto { Success = false, StatusCode = 401, Message = "Invalid Login Details" };
                }
                
                if (statusCode != 200 && statusCode != 1)
                {
                    return new ClientLoginResponseDto { Success = false, StatusCode = 400, Message = message != "" ? message : "Authentication failed at external API." };
                }
                
                // Even if Status_Code is 200, we MUST extract User_Locations
                List<LocationDto> locations = new List<LocationDto>();
                
                if (root.TryGetProperty("User_Locations", out var userLocsElement) && userLocsElement.ValueKind == JsonValueKind.Array)
                {
                    locations = JsonSerializer.Deserialize<List<LocationDto>>(userLocsElement.GetRawText()) ?? new List<LocationDto>();
                }
                else if (root.TryGetProperty("Response_Body", out var respBodyElement))
                {
                    if (respBodyElement.ValueKind == JsonValueKind.Array && respBodyElement.GetArrayLength() > 0)
                    {
                        var firstItem = respBodyElement[0];
                        if (firstItem.TryGetProperty("User_Locations", out var innerLocs) && innerLocs.ValueKind == JsonValueKind.Array)
                        {
                            locations = JsonSerializer.Deserialize<List<LocationDto>>(innerLocs.GetRawText()) ?? new List<LocationDto>();
                        }
                    }
                    else if (respBodyElement.ValueKind == JsonValueKind.Object)
                    {
                        if (respBodyElement.TryGetProperty("User_Locations", out var objLocs) && objLocs.ValueKind == JsonValueKind.Array)
                        {
                            locations = JsonSerializer.Deserialize<List<LocationDto>>(objLocs.GetRawText()) ?? new List<LocationDto>();
                        }
                    }
                }

                if (locations == null || locations.Count == 0)
                {
                    _logger.LogWarning($"External API returned Status 200 but User_Locations is missing or empty for {request.Email}");
                    return new ClientLoginResponseDto { Success = false, StatusCode = 400, Message = "Authentication succeeded but User_Locations is missing or malformed." };
                }

                // Authentication succeeded and locations are present. Save them.
                var saveRequest = new SaveLocationsRequest
                {
                    Company_Code = request.Email,
                    User_Locations = locations
                };

                await _locationService.SaveLocationsAsync(saveRequest);

                return new ClientLoginResponseDto 
                { 
                    Success = true, 
                    StatusCode = 200,
                    Message = "Login successful.",
                    CompanyCode = request.Email
                };
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("External API request timed out.");
                return new ClientLoginResponseDto { Success = false, StatusCode = 504, Message = "The authentication service timed out. Please try again." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during external API authentication.");
                string exactError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ClientLoginResponseDto { Success = false, StatusCode = 500, Message = $"Server Error: {exactError}" };
            }
        }
    }
}
