using EnhanzerAssessment.Api.DTOs;

namespace EnhanzerAssessment.Api.Services
{
    public interface IAuthService
    {
        Task<ClientLoginResponseDto> AuthenticateAsync(LoginRequestDto request);
    }
}
