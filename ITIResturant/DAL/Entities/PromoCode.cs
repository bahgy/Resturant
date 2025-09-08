using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class PromoCode
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[StringLength(50)]  >>>>>>>>>>>>>>>>> Fluant Api
        //[Index(IsUnique = true)]  >>>>>>>>>>>>>> EF 6 
        public string Code { get; set; }
        public string Description { get; set; }
        public string discount { get; set; }
        public DateTime ValidFromTime { get; set; }
        public DateTime ValidTo { get; set; }
        public int MaxUsedtime { get; set; }
        public int UsedCount { get; set; }


    }
}
