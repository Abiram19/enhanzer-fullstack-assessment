using System;
using System.Threading.Tasks;
using EnhanzerAssessment.Api.DTOs;
using EnhanzerAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerAssessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseBillsController : ControllerBase
    {
        private readonly PurchaseBillService _service;

        public PurchaseBillsController(PurchaseBillService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseBill()
        {
            using var reader = new System.IO.StreamReader(Request.Body);
            var rawBody = await reader.ReadToEndAsync();
            Console.WriteLine("RAW REQUEST BODY:");
            Console.WriteLine(rawBody);

            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var request = System.Text.Json.JsonSerializer.Deserialize<CreatePurchaseBillDto>(rawBody, options);

            if (request == null || request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new { message = "Validation failed", errors = new[] { "Request body or Items is empty" } });
            }

            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(request);
            var validationResults = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(request, validationContext, validationResults, true);

            foreach (var item in request.Items)
            {
                var itemContext = new System.ComponentModel.DataAnnotations.ValidationContext(item);
                if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(item, itemContext, validationResults, true))
                {
                    isValid = false;
                }
            }

            if (!isValid)
            {
                var errors = validationResults.Select(e => e.ErrorMessage);
                Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
                return BadRequest(new { message = "Validation failed", errors });
            }

            try
            {
                var bill = await _service.CreatePurchaseBillAsync(request);
                return Ok(new { message = "Purchase bill created successfully", id = bill.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // In production, log the exception
                return StatusCode(500, new { message = "An error occurred while saving the purchase bill.", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseBills([FromQuery] string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return BadRequest(new { message = "CompanyCode is required." });
            }

            var bill = await _service.GetLatestPurchaseBillAsync(companyCode);

            if (bill == null)
            {
                return Ok(new { message = "No items added yet." }); // Per requirements, handle gracefully without 404 error logs
            }

            return Ok(bill);
        }
    }
}
