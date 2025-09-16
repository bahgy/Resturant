using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVM.PromoCode
{
    public class PromoCodeVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountValue { get; set; }
        public string DiscountType { get; set; } // "Percentage" or "FixedAmount"
        public string DiscountDisplay => DiscountType == "Percentage" ? $"{DiscountValue}%" : $"{DiscountValue:C}";
        public DateTime ValidFromTime { get; set; }
        public DateTime ValidTo { get; set; }
        public int MaxUsedTime { get; set; }
        public int UsedCount { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public bool IsActive => DateTime.UtcNow >= ValidFromTime && DateTime.UtcNow <= ValidTo && UsedCount < MaxUsedTime;
        public int RemainingUses => MaxUsedTime - UsedCount;
    }
}
