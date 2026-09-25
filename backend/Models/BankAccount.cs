using System.ComponentModel.DataAnnotations;

namespace EnhanzerAssessment.Api.Models
{
    public class BankAccount
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string UpdatedTime { get; set; }
        public bool? Reviewed { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public string FormattedBalance { get; set; }
        public bool? IsNegative { get; set; }
    }
}
