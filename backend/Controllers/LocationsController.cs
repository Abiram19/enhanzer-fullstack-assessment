using EnhanzerAssessment.Api.DTOs;
using EnhanzerAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerAssessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveLocations([FromBody] SaveLocationsRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Company_Code))
            {
                return BadRequest("Company_Code is required.");
            }

            await _locationService.SaveLocationsAsync(request);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationDto>>> GetLocations([FromQuery] string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return BadRequest("companyCode query parameter is required.");
            }

            var locations = await _locationService.GetLocationsAsync(companyCode);
            return Ok(locations);
        }
    }
}
