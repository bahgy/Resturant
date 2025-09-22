namespace Restaurant.BLL.ModelVM.CartVM
{
    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new();
        public decimal Subtotal => Items.Sum(i => i.TotalPrice);
        public decimal Tax => Subtotal * 0.085m;
        public decimal DeliveryFee => 5m;
        public decimal GrandTotal => Subtotal + Tax + DeliveryFee;
    }
}
