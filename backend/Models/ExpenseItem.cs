using System.ComponentModel.DataAnnotations;

namespace EnhanzerAssessment.Api.Models
{
    public class ExpenseItem
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string FormattedAmount { get; set; }
        public string Color { get; set; }
        public int Percentage { get; set; }
    }
}
