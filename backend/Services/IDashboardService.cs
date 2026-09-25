using System.Threading.Tasks;
using EnhanzerAssessment.Api.DTOs;

namespace EnhanzerAssessment.Api.Services
{
    public interface IDashboardService
    {
        Task<DashboardDataDto> GetDashboardDataAsync(string companyCode);
    }
}
