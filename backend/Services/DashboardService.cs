using System;
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

                // 1. Widget 01 - Table View: Latest 5 Purchase Orders
                var latestPOs = await _context.PurchaseBills
                    .Where(b => b.CompanyCode == companyCode)
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(5)
                    .Select(b => new LatestPurchaseOrderDto
                    {
                        Id = b.Id,
                        NoOfItems = b.Items.Count,
                        NetAmount = b.Items.Sum(i => i.TotalCost) 
                    })
                    .ToListAsync();

                // 2. Widget 02 - List View: Oldest 10 Purchase Order Items
                var oldestItems = await _context.PurchaseBillItems
                    .Where(i => i.PurchaseBill.CompanyCode == companyCode)
                    .OrderBy(i => i.PurchaseBill.CreatedAt)
                    .Take(10)
                    .Select(i => new OldestPurchaseOrderItemDto
                    {
                        PurchaseOrderId = i.PurchaseBillId,
                        ItemName = i.ItemName,
                        NoOfQuantity = i.Qty
                    })
                    .ToListAsync();

                // 3. Widget 03 - Donut Chart: All Purchase Order Items grouped by Item Name and Total Quantity
                var chartData = await _context.PurchaseBillItems
                    .Where(i => i.PurchaseBill.CompanyCode == companyCode)
                    .GroupBy(i => i.ItemName)
                    .Select(g => new PurchaseOrderItemChartDto
                    {
                        ItemName = g.Key,
                        TotalQuantity = g.Sum(i => i.Qty)
                    })
                    .ToListAsync();

                _logger.LogInformation("Successfully retrieved dashboard data.");

                return new DashboardDataDto
                {
                    LatestPurchaseOrders = latestPOs,
                    OldestPurchaseOrderItems = oldestItems,
                    PurchaseOrderItemChart = chartData
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
