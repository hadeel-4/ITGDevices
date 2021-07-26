using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public String rolename { get; set; }
        public ICollection<UserRole> userRole { get; set; }
    }
}
