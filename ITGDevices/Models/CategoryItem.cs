using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class CategoryItem
    {
        [Key]
        public int ID { get; set; }
        public int CategoryID { get; set; }

        public int ItemID { get; set; }
    }
}
