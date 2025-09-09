using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    { 
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Phone { get; set; }
        public DateTime Joindate { get; set; }
      
        
    }
}
