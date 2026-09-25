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

        // Consistent colour palette for the donut chart slices
        private static readonly string[] SliceColors =
        {
            "#0072ce", "#00a8cc", "#10b981", "#f59e0b",
            "#8b5cf6", "#ef4444", "#0284c7", "#0f3d59"
        };

        public DashboardService(ApplicationDbContext context, ILogger<DashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DashboardDataDto> GetDashboardDataAsync(string companyCode)
        {
            try
            {
                _logger.LogInformation("Fetching dashboard data for CompanyCode: {CompanyCode}", companyCode);

                // ── Widget 01 : Latest 5 Purchase Orders ─────────────────────────────
                var latestPOs = await _context.PurchaseBills
                    .Where(b => b.CompanyCode == companyCode)
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(5)
                    .Select(b => new LatestPORowDto
                    {
                        Id        = b.Id,
                        NoOfItems = b.Items != null ? b.Items.Count : 0,
                        NetAmount = b.Items != null ? b.Items.Sum(i => i.TotalCost) : 0m
                    })
                    .ToListAsync();

                // ── Widget 02 : Oldest 10 Purchase Order Items ────────────────────────
                var oldestItems = await _context.PurchaseBillItems
                    .Where(i => i.PurchaseBill != null && i.PurchaseBill.CompanyCode == companyCode)
                    .OrderBy(i => i.PurchaseBill!.CreatedAt)
                    .Take(10)
                    .Select(i => new OldestPOItemRowDto
                    {
                        PurchaseOrderId = i.PurchaseBillId,
                        ItemName        = i.ItemName,
                        NoOfQuantity    = i.Qty
                    })
                    .ToListAsync();

                // ── Widget 03 : Items grouped by name (Donut Chart) ───────────────────
                var rawChart = await _context.PurchaseBillItems
                    .Where(i => i.PurchaseBill != null && i.PurchaseBill.CompanyCode == companyCode)
                    .GroupBy(i => i.ItemName)
                    .Select(g => new { ItemName = g.Key, TotalQuantity = g.Sum(i => i.Qty) })
                    .OrderByDescending(g => g.TotalQuantity)
                    .ToListAsync();

                int totalQty = rawChart.Sum(c => c.TotalQuantity);

                var itemChart = rawChart.Select((item, idx) => new ItemChartSliceDto
                {
                    ItemName      = item.ItemName,
                    TotalQuantity = item.TotalQuantity,
                    Color         = SliceColors[idx % SliceColors.Length],
                    Percentage    = totalQty > 0
                                      ? Math.Round((double)item.TotalQuantity / totalQty * 100, 1)
                                      : 0
                }).ToList();

                _logger.LogInformation(
                    "Dashboard built – POs: {POCount}, Items: {ItemCount}, ChartSlices: {SliceCount}",
                    latestPOs.Count, oldestItems.Count, itemChart.Count);

                return new DashboardDataDto
                {
                    LatestPurchaseOrders  = latestPOs,
                    OldestPurchaseOrderItems = oldestItems,
                    ItemChart             = itemChart,
                    TotalItemQuantity     = totalQty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error building dashboard data.");
                throw;
            }
        }
    }
}
