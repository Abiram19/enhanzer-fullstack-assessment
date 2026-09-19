using EnhanzerAssessment.Api.Data;
using EnhanzerAssessment.Api.DTOs;
using EnhanzerAssessment.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerAssessment.Api.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveLocationsAsync(SaveLocationsRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Company_Code) || request.User_Locations == null || !request.User_Locations.Any())
            {
                return;
            }

            foreach (var locDto in request.User_Locations)
            {
                if (string.IsNullOrWhiteSpace(locDto.Location_Code) || string.IsNullOrWhiteSpace(locDto.Location_Name))
                    continue;

                var existing = await _context.Location_Details
                    .FirstOrDefaultAsync(l => l.Company_Code == request.Company_Code && l.Location_Code == locDto.Location_Code);

                if (existing == null)
                {
                    _context.Location_Details.Add(new LocationDetail
                    {
                        Company_Code = request.Company_Code,
                        Location_Code = locDto.Location_Code,
                        Location_Name = locDto.Location_Name
                    });
                }
                else
                {
                    // Update if location name changed
                    if (existing.Location_Name != locDto.Location_Name)
                    {
                        existing.Location_Name = locDto.Location_Name;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<LocationDto>> GetLocationsAsync(string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return new List<LocationDto>();
            }

            return await _context.Location_Details
                .Where(l => l.Company_Code == companyCode)
                .Select(l => new LocationDto
                {
                    Location_Code = l.Location_Code,
                    Location_Name = l.Location_Name
                })
                .ToListAsync();
        }
    }
}
