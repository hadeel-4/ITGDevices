using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ITGDevices.Models
{
    public class UserItemRequest
    {


        [Key]
        public System.Guid ID { get; set; }

        public int UserID { get; set; }
        public int ItemID { get; set; }
    }
}
