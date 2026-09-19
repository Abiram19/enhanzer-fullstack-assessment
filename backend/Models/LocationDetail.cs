namespace EnhanzerAssessment.Api.Models
{
    public class LocationDetail
    {
        public string Company_Code { get; set; } = string.Empty;
        public string Location_Code { get; set; } = string.Empty;
        public string Location_Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
