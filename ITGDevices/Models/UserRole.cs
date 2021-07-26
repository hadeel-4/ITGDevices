using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class UserRole
    {
        [Key]
        public int ID { get; set; }
        public int userID { get; set; }

        public int roleID { get; set; }
    }
}
