namespace EnhanzerAssessment.Api.DTOs
{
    public class LocationDto
    {
        public string Location_Code { get; set; } = string.Empty;
        public string Location_Name { get; set; } = string.Empty;
    }

    public class SaveLocationsRequest
    {
        public string Company_Code { get; set; } = string.Empty;
        public List<LocationDto> User_Locations { get; set; } = new();
    }
}
