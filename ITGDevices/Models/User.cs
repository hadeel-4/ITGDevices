using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public String username { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public String Email { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public String FirstName { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public String LastName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords are different")]
        [DataType(DataType.Password)]
        public String CoPassword { get; set; }

        public UserRole userRole { get; set; }

        public ICollection<UserItem> UserItem { get; set; }
    }
}
