using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DAL.Enum;

namespace DAL.Entities
{
    public class PromoCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Required]
        public DateTime ValidFromTime { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public int MaxUsedtime { get; set; }

        public int UsedCount { get; set; }

        public decimal MinimumOrderAmount { get; set; }


        public List<Order> Orders { get; set; }
    }
}
