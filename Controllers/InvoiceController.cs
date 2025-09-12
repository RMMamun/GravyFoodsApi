using GravyFoodsApi.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetInvoice(int id)
        {
            var model = new InvoiceModel
            {
                InvoiceNumber = $"INV-{id}",
                CustomerName = "Jane Smith",
                Date = DateTime.Now,
                Items = new List<InvoiceItem>
            {
                new() { Name = "Service A", Quantity = 1, Price = 100 }
            }
            };

            var document = new InvoiceDocument(model);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Invoice_{id}.pdf");
        }
    }
}
