
namespace Restaurant.BLL.ModelVMOrderItem
{
    public class OrderItemVM
    {
        public OrderItemVM() { }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
    }
}
