using GravyFoodsApi.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using GravyFoodsApi.MasjidServices;
using System.Threading.Tasks;
using GravyFoodsApi.Models;
using GravyFoodsApi.MasjidRepository;



namespace GravyFoodsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {

        private readonly SalesService _salesService;

        public InvoiceController(SalesService salesService)
        {
            _salesService = salesService;
        }


        [HttpGet("{invoiceNo}")]
        public async Task<IActionResult> GetInvoice(string invoiceNo)
        {
            
            //SalesService salesService = new SalesService();

            var salse = await _salesService.GetSaleByIdAsync(invoiceNo);
            if (salse != null)
            {

                //var model = new InvoiceModel
                //{
                //    InvoiceNumber = $"INV-{invoiceNo}",
                //    CustomerName = "Jane Smith",
                //    Date = salse.CreatedDateTime,
                //Items = new List<InvoiceItem>
                //    {
                //    new() { Name = "Service A", Quantity = 1, Price = 100 }
                //    }
                //};


                var model = new InvoiceModel
                {
                    InvoiceNumber = $"INV-{invoiceNo}",
                    CustomerName = salse.CustomerInfo.CustomerName,  // assuming your Sale object has this
                    Date = salse.CreatedDateTime,
                    Items = salse.SalesDetails.Select(d => new InvoiceItem
                    {
                        Name = d.Product.Name,   // or d.ProductName if you want names
                        Quantity = d.Quantity,
                        Price = (decimal)d.PricePerUnit,
                        TotalPrice = d.TotalPrice
                    }).ToList()
                };


                var document = new InvoiceDocument(model);
                var pdfBytes = document.GeneratePdf();

                return File(pdfBytes, "application/pdf", $"Invoice_{invoiceNo}.pdf");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
