using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Restaurant.BLL.ModelVM.PromoCode
{
    public class PromoCodeValidationVM
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountDisplay { get; set; }

        public Restaurant.DAL.Entities.PromoCode PromoCode { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
