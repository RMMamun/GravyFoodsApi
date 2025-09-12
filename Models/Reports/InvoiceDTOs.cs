namespace GravyFoodsApi.Models.Reports
{
    public class InvoiceModel
    {
        public string InvoiceNumber { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public List<InvoiceItem> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    }

    public class InvoiceItem
    {
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
