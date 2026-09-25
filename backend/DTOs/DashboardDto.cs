using System.Collections.Generic;

namespace EnhanzerAssessment.Api.DTOs
{
    // ── Widget 01 : Latest Purchase Orders (Table) ──────────────────────
    public class LatestPORowDto
    {
        public int Id { get; set; }
        public decimal NetAmount { get; set; }
        public int NoOfItems { get; set; }
    }

    // ── Widget 02 : Oldest Purchase Order Items (List) ───────────────────
    public class OldestPOItemRowDto
    {
        public int PurchaseOrderId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int NoOfQuantity { get; set; }
    }

    // ── Widget 03 : Item Quantity Distribution (Donut Chart) ─────────────
    public class ItemChartSliceDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public string Color { get; set; } = string.Empty;
        public double Percentage { get; set; }
    }

    // ── Top-level response ────────────────────────────────────────────────
    public class DashboardDataDto
    {
        /// <summary>Latest 5 Purchase Orders (Widget 01 – Table View)</summary>
        public List<LatestPORowDto> LatestPurchaseOrders { get; set; } = new();

        /// <summary>Oldest 10 Purchase Order Items (Widget 02 – List View)</summary>
        public List<OldestPOItemRowDto> OldestPurchaseOrderItems { get; set; } = new();

        /// <summary>All items grouped by name with total quantity (Widget 03 – Donut Chart)</summary>
        public List<ItemChartSliceDto> ItemChart { get; set; } = new();

        /// <summary>Grand total quantity – used as the donut centre label</summary>
        public int TotalItemQuantity { get; set; }
    }
}
