using System.ComponentModel.DataAnnotations;

namespace EnhanzerAssessment.Api.DTOs
{
    public class CreatePurchaseBillItemDto
    {
        [Required]
        [MaxLength(50)]
        public string Item { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Batch { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Qty must be at least 1.")]
        public int Qty { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Standard Cost cannot be negative.")]
        public decimal StandardCost { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Standard Price cannot be negative.")]
        public decimal StandardPrice { get; set; }

        [Range(typeof(decimal), "0", "100", ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal Discount { get; set; }
    }
}
