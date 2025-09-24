using System.ComponentModel.DataAnnotations;
using Restaurant.DAL.Enum;

namespace Resturant.BLL.ModelVm.PaymentVm
{
    public class CreatePaymentVm
    {
        [Required]
        [Display(Name = "Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }  // Enum مباشر

        [Required]
        public int OrderId { get; set; }
    }
}
