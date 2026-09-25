using System.ComponentModel.DataAnnotations;

namespace EnhanzerAssessment.Api.Models
{
    public class DelayedOrder
    {
        [Key]
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string Product { get; set; }
        public string DueDate { get; set; }
        public int DaysLate { get; set; }
        public string Urgency { get; set; }
    }
}
