using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Entities
{
    public class Delivery : AppUser
    {
        public string? VehicleType { get; set; }
        public string? VehicleNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public ICollection<Order> Orders { get; set; }
        public bool IsAvailable { get; set; } = true;

    }
}
