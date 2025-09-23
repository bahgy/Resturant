using System.ComponentModel.DataAnnotations;
using Restaurant.DAL.Enum;

namespace Resturant.BLL.ModelVM.PaymentVM
{
    public class EditPaymentVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Payment Status")]
        public PaymentStatus Status { get; set; }
    }
}
