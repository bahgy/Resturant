using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Enum
{
        public enum OrderStatus
        {
            Pending = 0,
            Processing = 1,
            Shipped = 2,
            Delivered = 3,
            Cancelled = 4
        }
}
