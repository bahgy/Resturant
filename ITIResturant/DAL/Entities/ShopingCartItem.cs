 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Entities
{
    public class ShopingCartItem
    {
      

        [Key]
        public int Id { get; set; }

        public decimal Quantity { get; set; }

        // Cart relationship
        public int CartId { get; set; }
        public Cart Cart { get; set; }  // Navigation Property

        // Product relationship
        public int ProductId { get; set; }
        public Product Product { get; set; }  // Navigation Property
    }

}
