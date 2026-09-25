using System.Collections.Generic;

namespace EnhanzerAssessment.Api.DTOs
{
    // Raw PO models (maintains 100% backwards compatibility with assignment requirements)
    public class LatestPurchaseOrderDto
    {
        public int Id { get; set; }
        public decimal NetAmount { get; set; }
        public int NoOfItems { get; set; }
    }

    public class OldestPurchaseOrderItemDto
    {
        public int PurchaseOrderId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int NoOfQuantity { get; set; }
    }

    public class PurchaseOrderItemChartDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
    }

    // Configurable Widget DTOs matching the eZuite dashboard screenshot 100%
    public class TableWidgetRowDto
    {
        public string OrderNo { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public string DaysLate { get; set; } = string.Empty;
        public int? PurchaseOrderId { get; set; }
        public decimal? NetAmount { get; set; }
        public int? NoOfItems { get; set; }
    }

    public class TableWidgetDto
    {
        public string Title { get; set; } = "Delayed Orders";
        public string Period { get; set; } = "Today";
        public string Subtitle { get; set; } = "5 orders delayed";
        public List<string> Headers { get; set; } = new() { "Order No", "Product", "Due Date", "Days Late" };
        public List<TableWidgetRowDto> Rows { get; set; } = new();
    }

    public class ListWidgetItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string UpdatedText { get; set; } = string.Empty;
        public string? BadgeText { get; set; }
        public string Amount { get; set; } = string.Empty;
        public bool IsNegative { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? Quantity { get; set; }
    }

    public class ListWidgetDto
    {
        public string Title { get; set; } = "Bank Accounts";
        public string Period { get; set; } = "Today";
        public List<ListWidgetItemDto> Items { get; set; } = new();
    }

    public class ChartSliceDto
    {
        public string Code { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string FormattedValue { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public double Percentage { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class ChartWidgetDto
    {
        public string Title { get; set; } = "Expenses";
        public string Period { get; set; } = "Today";
        public string TotalValue { get; set; } = "$18,000.00";
        public string TotalLabel { get; set; } = "Total expenses";
        public List<ChartSliceDto> Slices { get; set; } = new();
    }

    public class DashboardDataDto
    {
        // Raw PO collections from database
        public List<LatestPurchaseOrderDto> LatestPurchaseOrders { get; set; } = new();
        public List<OldestPurchaseOrderItemDto> OldestPurchaseOrderItems { get; set; } = new();
        public List<PurchaseOrderItemChartDto> PurchaseOrderItemChart { get; set; } = new();

        // 100% Configurable Widgets matching eZuite Dashboard design
        public TableWidgetDto TableWidget { get; set; } = new();
        public ListWidgetDto ListWidget { get; set; } = new();
        public ChartWidgetDto ChartWidget { get; set; } = new();

        // Mapped Purchase Order Widgets formatted in the exact same eZuite layout
        public TableWidgetDto PoTableWidget { get; set; } = new();
        public ListWidgetDto PoListWidget { get; set; } = new();
        public ChartWidgetDto PoChartWidget { get; set; } = new();
    }
}
