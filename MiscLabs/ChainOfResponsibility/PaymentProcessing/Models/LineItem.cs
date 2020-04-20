namespace PaymentProcessing.Models
{
    public class LineItem
    {
        public string Id { get; }
        public string Name { get; }
        public decimal Price { get; }

        public LineItem(string id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}