using ITGDevices.Data;
using ITGDevices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Controllers
{
    public class OperationsManagerController : Controller
    {
        private readonly DeviceContext _context;


        public OperationsManagerController(DeviceContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "OperationsManager", true) == 0)
            {
                var userItems = _context.UserItem.Where(i=>i.UserID== (int)HttpContext.Session.GetInt32("id"));
                List<Item> items = new List<Item>();
               foreach(UserItem userItem in userItems)
                {
                    Item item = _context.Items.Single(i => i.ID == userItem.ItemID);
                    items.Add(item);

                }

                return View(items);
            }
            else return RedirectToAction("Login", "users");
        }
    }
}
