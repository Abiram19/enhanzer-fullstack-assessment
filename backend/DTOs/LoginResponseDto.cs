using System.Text.Json.Serialization;

namespace EnhanzerAssessment.Api.DTOs
{
    public class ExternalLoginResponseDto
    {
        public int Status_Code { get; set; }
        public string? Sync_Time { get; set; }
        public string? Message { get; set; }
        
        // This handles cases where Response_Body could be null, an object, or an array
        [JsonIgnore]
        public object? RawResponseBody { get; set; }
        
        // Note: We parse User_Locations manually based on the structure since the API is inconsistent
        public List<LocationDto>? User_Locations { get; set; }
    }

    public class ClientLoginResponseDto
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; } = 400;
        public string Message { get; set; } = string.Empty;
        public string? CompanyCode { get; set; }
    }
}
