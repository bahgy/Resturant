

namespace Restaurant.BLL.ModelVMPromoCode
{
    public class CreatePromoCodeVM
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount value must be greater than 0")]
        public decimal DiscountValue { get; set; }

        [Required]
        public string DiscountType { get; set; } // "Percentage" or "FixedAmount"

        [Required]
        public DateTime ValidFromTime { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Max uses must be at least 1")]
        public int MaxUsedTime { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinimumOrderAmount { get; set; } = 0;
    }
}

