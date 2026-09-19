using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnhanzerAssessment.Api.Models
{
    public class PurchaseBill
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string? CompanyCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PurchaseBillItem> Items { get; set; } = new List<PurchaseBillItem>();
    }
}
