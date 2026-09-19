using EnhanzerAssessment.Api.DTOs;

namespace EnhanzerAssessment.Api.Services
{
    public interface ILocationService
    {
        Task SaveLocationsAsync(SaveLocationsRequest request);
        Task<List<LocationDto>> GetLocationsAsync(string companyCode);
    }
}
