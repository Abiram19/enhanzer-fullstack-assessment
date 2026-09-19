using EnhanzerAssessment.Api.DTOs;
using EnhanzerAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerAssessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ClientLoginResponseDto { Success = false, Message = "Email and Password are required." });
            }

            var result = await _authService.AuthenticateAsync(request);

            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}
