using System.Collections.Generic;

namespace EnhanzerAssessment.Api.DTOs
{
    public class LatestPurchaseOrderDto
    {
        public int Id { get; set; }
        public decimal NetAmount { get; set; }
        public int NoOfItems { get; set; }
    }

    public class OldestPurchaseOrderItemDto
    {
        public int PurchaseOrderId { get; set; }
        public string ItemName { get; set; }
        public int NoOfQuantity { get; set; }
    }

    public class PurchaseOrderItemChartDto
    {
        public string ItemName { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class DashboardDataDto
    {
        public List<LatestPurchaseOrderDto> LatestPurchaseOrders { get; set; }
        public List<OldestPurchaseOrderItemDto> OldestPurchaseOrderItems { get; set; }
        public List<PurchaseOrderItemChartDto> PurchaseOrderItemChart { get; set; }
    }
}
