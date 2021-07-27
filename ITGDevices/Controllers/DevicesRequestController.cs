using ITGDevices.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGDevices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITGDevices.Controllers
{
    public class DevicesRequestController : Controller
    {
        private readonly DeviceContext _context;


        public DevicesRequestController(DeviceContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Employee", true) == 0)
            {

                return View(await _context.Items.ToListAsync());
            }
            else return RedirectToAction("Login", "users");

        }

        public async Task<IActionResult> Details(int? id)
        {
            if ( (string.Compare(HttpContext.Session.GetString("role"), "Employee", true) == 0))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var item = await _context.Items
                    .FirstOrDefaultAsync(m => m.ID == id);//
                if (item == null)
                {
                    return NotFound();
                }
                CategoryItem categoryItem = _context.CategoryItem.Single(c => c.ItemID == item.ID);
                Category category = _context.Category.Single(c => c.ID == categoryItem.CategoryID);//
                UserItem h = _context.UserItem.Single(i => i.ItemID == item.ID);
                User holder = _context.users.Single(i => i.ID == h.UserID);
                ItemOperation itemOperation = new ItemOperation();
                itemOperation.category = category;
                itemOperation.item = item;
                itemOperation.holder = holder;




                return View(itemOperation);

                // return View(user);
            }
            else return RedirectToAction("Login", "users");
        }
    }
}
