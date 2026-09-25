using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EnhanzerAssessment.Api.Services;
using EnhanzerAssessment.Api.DTOs;

namespace EnhanzerAssessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDataDto>> GetDashboardData([FromQuery] string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return BadRequest(new { message = "CompanyCode is required." });
            }

            try
            {
                _logger.LogInformation("GET request received for dashboard data.");
                var data = await _dashboardService.GetDashboardDataAsync(companyCode);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve dashboard data in controller.");
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard data." });
            }
        }
    }
}
