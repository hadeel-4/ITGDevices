using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public String Name { get; set; }

        public String Description { get; set; }


        public  ICollection<CategoryItem> CategoryItem { get; set; }
    }
}
