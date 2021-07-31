using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Models
{
    public class ItemOperation
    {
        public Item item { get; set; } //
        public int ItemID { get; set; } 
        public String ItemName { get; set; }
        public  Category category { get; set; }
        public List<Category> categories { get; set; }//
        public int CategoryID { get; set; }//
        public List<User> managers { get; set; } //
        public User holder { get; set; }
        public int ManagerID { get; set; }//
        public int CId { get; set; }//
        public int UserId { get; set; }//
        public User requester { get; set; }//
        public UserItemRequest UserItemRequest { get; set; }
        public System.Guid UserItemRequestId { get; set; }//
        public User user { get; set; }//



        public List<Item> Items { get; set; }
     
       
        public CategoryItem CategoryItem { get; set; }//
    }
}
