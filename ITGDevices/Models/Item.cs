using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class Item
    {
         [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int SerialNumber { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public String Name { get; set; }
        
        
        
        public String Description { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public String Manufacturer { get; set; }
        [Required(ErrorMessage = "This field is required")]
        
        public String Model { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public bool IsDeliver { get; set; }

        [DataType(dataType:DataType.Date)]
        public DateTime PurchaseDate { get; set; }



        public CategoryItem CategoryItem { get; set; }
        public UserItem UserItem { get; set; }
        public ICollection<UserItemRequest> UserItemRequest { get; set; }

    }
}
