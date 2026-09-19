using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnhanzerAssessment.Api.DTOs
{
    public class CreatePurchaseBillDto
    {
        [MaxLength(100)]
        public string? CompanyCode { get; set; }

        public List<CreatePurchaseBillItemDto> Items { get; set; } = new List<CreatePurchaseBillItemDto>();
    }
}
