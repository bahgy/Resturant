
namespace Restaurant.BLL.ModelVM.MenuVM
{
    public class MenuVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductVM> Products { get; set; }
    }
}
