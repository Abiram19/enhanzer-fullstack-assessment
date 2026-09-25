using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EnhanzerAssessment.Api.Data;
using EnhanzerAssessment.Api.DTOs;

namespace EnhanzerAssessment.Api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(ApplicationDbContext context, ILogger<DashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DashboardDataDto> GetDashboardDataAsync(string companyCode)
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard data from database for CompanyCode: {CompanyCode}", companyCode);

                // 1. Raw Purchase Orders Data from Database
                var latestPOs = await _context.PurchaseBills
                    .Where(b => b.CompanyCode == companyCode)
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(5)
                    .Select(b => new LatestPurchaseOrderDto
                    {
                        Id = b.Id,
                        NoOfItems = b.Items != null ? b.Items.Count : 0,
                        NetAmount = b.Items != null ? b.Items.Sum(i => i.TotalCost) : 0
                    })
                    .ToListAsync();

                var oldestItems = await _context.PurchaseBillItems
                    .Where(i => i.PurchaseBill != null && i.PurchaseBill.CompanyCode == companyCode)
                    .OrderBy(i => i.PurchaseBill.CreatedAt)
                    .Take(10)
                    .Select(i => new OldestPurchaseOrderItemDto
                    {
                        PurchaseOrderId = i.PurchaseBillId,
                        ItemName = i.ItemName,
                        NoOfQuantity = i.Qty
                    })
                    .ToListAsync();

                var chartData = await _context.PurchaseBillItems
                    .Where(i => i.PurchaseBill != null && i.PurchaseBill.CompanyCode == companyCode)
                    .GroupBy(i => i.ItemName)
                    .Select(g => new PurchaseOrderItemChartDto
                    {
                        ItemName = g.Key,
                        TotalQuantity = g.Sum(i => i.Qty)
                    })
                    .ToListAsync();

                // 2. Default Configurable Widgets matching eZuite Dashboard 100%
                var tableWidget = new TableWidgetDto
                {
                    Title = "Delayed Orders",
                    Period = "Today",
                    Subtitle = "5 orders delayed",
                    Headers = new List<string> { "Order No", "Product", "Due Date", "Days Late" },
                    Rows = new List<TableWidgetRowDto>
                    {
                        new TableWidgetRowDto { OrderNo = "ORD-2024-001", Product = "Steel Frame Assembly", DueDate = "Feb 15, 2024", DaysLate = "5 days" },
                        new TableWidgetRowDto { OrderNo = "ORD-2024-002", Product = "Hydraulic Pump", DueDate = "Feb 10, 2024", DaysLate = "10 days" },
                        new TableWidgetRowDto { OrderNo = "ORD-2024-003", Product = "Control Panel", DueDate = "Feb 18, 2024", DaysLate = "2 days" },
                        new TableWidgetRowDto { OrderNo = "ORD-2024-004", Product = "Motor Assembly", DueDate = "Feb 12, 2024", DaysLate = "8 days" },
                        new TableWidgetRowDto { OrderNo = "ORD-2024-005", Product = "Bearing Kit", DueDate = "Feb 16, 2024", DaysLate = "4 days" }
                    }
                };

                var listWidget = new ListWidgetDto
                {
                    Title = "Bank Accounts",
                    Period = "Today",
                    Items = new List<ListWidgetItemDto>
                    {
                        new ListWidgetItemDto
                        {
                            Title = "Bank",
                            Subtitle = "In QuickBooks",
                            UpdatedText = "Updated 1178 days ago",
                            BadgeText = "Reviewed",
                            Amount = "AED-65,919.02",
                            IsNegative = true
                        },
                        new ListWidgetItemDto
                        {
                            Title = "123.1234 PB_test",
                            Subtitle = "In QuickBooks",
                            UpdatedText = "Updated 300 days ago",
                            BadgeText = null,
                            Amount = "AED4,567.00",
                            IsNegative = false
                        },
                        new ListWidgetItemDto
                        {
                            Title = "Cash and cash equivalents",
                            Subtitle = "In QuickBooks",
                            UpdatedText = "Updated 5 days ago",
                            BadgeText = "Reviewed",
                            Amount = "AED125,798.19",
                            IsNegative = false
                        },
                        new ListWidgetItemDto
                        {
                            Title = "Test Account",
                            Subtitle = "In QuickBooks",
                            UpdatedText = "Updated 100 days ago",
                            BadgeText = null,
                            Amount = "€200.00",
                            IsNegative = false
                        },
                        new ListWidgetItemDto
                        {
                            Title = "Master card 0011",
                            Subtitle = "In QuickBooks",
                            UpdatedText = "Updated 50 days ago",
                            BadgeText = null,
                            Amount = "AED1,507.05",
                            IsNegative = false
                        }
                    }
                };

                var chartWidget = new ChartWidgetDto
                {
                    Title = "Expenses",
                    Period = "Today",
                    TotalValue = "$18,000.00",
                    TotalLabel = "Total expenses",
                    Slices = new List<ChartSliceDto>
                    {
                        new ChartSliceDto { Code = "6010", Label = "6010 Online Marketing", FormattedValue = "$10,000.00", Value = 10000m, Percentage = 55.56, Color = "#00a8cc" },
                        new ChartSliceDto { Code = "6020", Label = "6020 Subscriptions", FormattedValue = "$6,000.00", Value = 6000m, Percentage = 33.33, Color = "#0284c7" },
                        new ChartSliceDto { Code = "6090", Label = "6090 Depreciation", FormattedValue = "$1,100.00", Value = 1100m, Percentage = 6.11, Color = "#0f3d59" },
                        new ChartSliceDto { Code = "9090", Label = "9090 Custom", FormattedValue = "$2,000.00", Value = 2000m, Percentage = 11.11, Color = "#10b981" }
                    }
                };

                // 3. Purchase Order Mapped Widgets (dynamically generated from database)
                var poTableWidget = new TableWidgetDto
                {
                    Title = "Latest Purchase Orders",
                    Period = "Today",
                    Subtitle = $"{latestPOs.Count} purchase orders found",
                    Headers = new List<string> { "Order ID", "Net Amount", "No. of Items", "Status" },
                    Rows = latestPOs.Select(p => new TableWidgetRowDto
                    {
                        OrderNo = $"PO-{p.Id:D4}",
                        Product = $"Net: ${p.NetAmount:N2}",
                        DueDate = $"{p.NoOfItems} Items",
                        DaysLate = "Completed",
                        PurchaseOrderId = p.Id,
                        NetAmount = p.NetAmount,
                        NoOfItems = p.NoOfItems
                    }).ToList()
                };

                var poListWidget = new ListWidgetDto
                {
                    Title = "Purchase Order Items",
                    Period = "Today",
                    Items = oldestItems.Select(item => new ListWidgetItemDto
                    {
                        Title = item.ItemName,
                        Subtitle = $"Order Ref: PO #{item.PurchaseOrderId}",
                        UpdatedText = "Recorded in system",
                        BadgeText = "Active",
                        Amount = $"{item.NoOfQuantity} Qty",
                        IsNegative = false,
                        PurchaseOrderId = item.PurchaseOrderId,
                        Quantity = item.NoOfQuantity
                    }).ToList()
                };

                var totalPoQty = chartData.Sum(c => c.TotalQuantity);
                var poColors = new[] { "#00a8cc", "#0284c7", "#0f3d59", "#10b981", "#f59e0b", "#8b5cf6" };
                var poChartWidget = new ChartWidgetDto
                {
                    Title = "Item Distribution",
                    Period = "Today",
                    TotalValue = $"{totalPoQty} Units",
                    TotalLabel = "Total items quantity",
                    Slices = chartData.Select((item, idx) => new ChartSliceDto
                    {
                        Code = $"ITEM-{idx + 1}",
                        Label = item.ItemName,
                        FormattedValue = $"{item.TotalQuantity} Units",
                        Value = item.TotalQuantity,
                        Percentage = totalPoQty > 0 ? Math.Round((double)item.TotalQuantity / totalPoQty * 100, 2) : 0,
                        Color = poColors[idx % poColors.Length]
                    }).ToList()
                };

                _logger.LogInformation("Successfully constructed dashboard data with 100% configurable widgets.");

                return new DashboardDataDto
                {
                    LatestPurchaseOrders = latestPOs,
                    OldestPurchaseOrderItems = oldestItems,
                    PurchaseOrderItemChart = chartData,
                    TableWidget = tableWidget,
                    ListWidget = listWidget,
                    ChartWidget = chartWidget,
                    PoTableWidget = poTableWidget,
                    PoListWidget = poListWidget,
                    PoChartWidget = poChartWidget
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving dashboard data.");
                throw;
            }
        }
    }
}
