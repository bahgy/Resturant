using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVM.Review
{
    public class ReviewVm
    {
        public int Id { get; set; }
        public string UserName { get; set; }  // comes from AppUser
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
