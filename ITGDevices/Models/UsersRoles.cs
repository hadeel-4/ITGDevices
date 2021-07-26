using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class UsersRoles
    {
        public User user { get; set; }
        public List<Role> roles { get; set; }

        public int roleId { get; set; }
    }
}
