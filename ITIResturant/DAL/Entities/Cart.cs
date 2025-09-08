using Azure.Core.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        // Foreign Key
        public int CustomerId { get; set; }

        // Navigation property
        public Customer Customer { get; set; }
        public List<ShopingCartItem> ShopingCartItem { get; set; }
    }

}
