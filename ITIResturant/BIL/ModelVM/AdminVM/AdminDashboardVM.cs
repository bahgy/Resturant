
using Restaurant.BLL.ModelVM.AdminVM;

namespace Restaurant.BLL.ModelVMAdminVM
{
    public class AdminDashboardVM
    {
        public int Users { get; set; }

        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }

        public decimal AverageOrderVal { get; set; }
        public decimal TotalIncome { get; set; }

        public int TotalProducts { get; set; }

        public List<TopProductVM> MostOrderedProducts { get; set; }

        
        public DateTime ReportStart { get; set; }
        public DateTime ReportEnd { get; set; }
    }

}
