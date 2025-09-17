using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVMPromoCode
{
    public class UpdatePromoCodeVM
    {
        [Required]
        public int Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? DiscountValue { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Code { get; set; } 
        public string DiscountType { get; set; }

        public DateTime? ValidFromTime { get; set; }

        public DateTime? ValidTo { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxUsedTime { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinimumOrderAmount { get; set; }
    }
}

