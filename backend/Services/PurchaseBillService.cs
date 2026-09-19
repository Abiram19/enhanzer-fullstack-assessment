using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnhanzerAssessment.Api.Data;
using EnhanzerAssessment.Api.DTOs;
using EnhanzerAssessment.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerAssessment.Api.Services
{
    public class PurchaseBillService
    {
        private readonly ApplicationDbContext _context;
        private readonly string[] _allowedItems = { "Mango", "Apple", "Banana", "Orange", "Grapes", "Kiwi", "Strawberry" };

        public PurchaseBillService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseBill> CreatePurchaseBillAsync(CreatePurchaseBillDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
            {
                throw new ArgumentException("Purchase bill must contain at least one item.");
            }

            var bill = new PurchaseBill
            {
                CompanyCode = dto.CompanyCode,
                CreatedAt = DateTime.UtcNow,
                Items = new List<PurchaseBillItem>()
            };

            foreach (var itemDto in dto.Items)
            {
                if (!_allowedItems.Contains(itemDto.Item))
                {
                    throw new ArgumentException($"Invalid item: {itemDto.Item}");
                }

                if (itemDto.Qty <= 0)
                {
                    throw new ArgumentException("Quantity must be greater than zero.");
                }
                if (itemDto.Discount < 0 || itemDto.Discount > 100)
                {
                    throw new ArgumentException("Discount must be between 0 and 100.");
                }

                // Recalculate based on assessment rules
                decimal totalCost = (itemDto.StandardCost * itemDto.Qty) * (1 - (itemDto.Discount / 100));
                decimal totalSelling = itemDto.StandardPrice * itemDto.Qty;

                var item = new PurchaseBillItem
                {
                    ItemName = itemDto.Item,
                    Batch = itemDto.Batch,
                    Qty = itemDto.Qty,
                    StandardCost = itemDto.StandardCost,
                    StandardPrice = itemDto.StandardPrice,
                    Discount = itemDto.Discount,
                    TotalCost = totalCost,
                    TotalSelling = totalSelling
                };

                bill.Items.Add(item);
            }

            _context.PurchaseBills.Add(bill);
            await _context.SaveChangesAsync();

            return bill;
        }

        public async Task<PurchaseBill?> GetLatestPurchaseBillAsync(string companyCode)
        {
            return await _context.PurchaseBills
                .Include(b => b.Items)
                .Where(b => b.CompanyCode == companyCode)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
