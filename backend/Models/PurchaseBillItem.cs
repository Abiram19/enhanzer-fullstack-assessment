using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnhanzerAssessment.Api.Models
{
    public class PurchaseBillItem
    {
        [Key]
        public int Id { get; set; }

        public int PurchaseBillId { get; set; }
        public PurchaseBill? PurchaseBill { get; set; }

        [MaxLength(50)]
        public string ItemName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Batch { get; set; }

        public int Qty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal StandardCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal StandardPrice { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSelling { get; set; }
    }
}
