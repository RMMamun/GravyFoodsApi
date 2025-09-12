namespace GravyFoodsApi.Models.Reports
{
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;

    public class InvoiceDocument : IDocument
    {
        private readonly InvoiceModel _model;

        public InvoiceDocument(InvoiceModel model)
        {
            _model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);

                page.Header().Text($"Invoice #{_model.InvoiceNumber}")
                    .FontSize(20).Bold();

                page.Content().Column(col =>
                {
                    col.Item().Text($"Customer: {_model.CustomerName}");
                    col.Item().Text($"Date: {_model.Date:dd MMM yyyy}");
                    col.Item().LineHorizontal(1);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(3);
                            c.RelativeColumn(1);
                            c.RelativeColumn(1);
                        });

                        // Table Header
                        table.Header(h =>
                        {
                            h.Cell().Text("Item").Bold();
                            h.Cell().AlignRight().Text("Qty").Bold();
                            h.Cell().AlignRight().Text("Price").Bold();
                        });

                        // Table Rows
                        foreach (var item in _model.Items)
                        {
                            table.Cell().Text(item.Name);
                            table.Cell().AlignRight().Text(item.Quantity.ToString());
                            table.Cell().AlignRight().Text($"${item.Price:F2}");
                        }
                    });

                    col.Item().LineHorizontal(1);
                    col.Item().AlignRight().Text($"Total: ${_model.Total:F2}").FontSize(14).Bold();
                });

                page.Footer().AlignCenter().Text("Thank you for your business!");
            });
        }
    }

}
