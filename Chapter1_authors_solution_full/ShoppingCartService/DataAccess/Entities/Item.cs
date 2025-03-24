namespace ShoppingCartService.DataAccess.Entities
{
    public class Item
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public uint Quantity { get; set; }
    }
}