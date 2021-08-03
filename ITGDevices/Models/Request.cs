using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class Request
    {
        public Item item { get; set; } //
        public int ItemID { get; set; }
        public UserItemRequest UserItemRequest { get; set; }
        public System.Guid UserItemRequestId { get; set; }//
    }
}
