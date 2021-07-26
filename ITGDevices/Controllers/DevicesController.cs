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
    public class DevicesController : Controller
    {
        private readonly DeviceContext _context;


        public DevicesController(DeviceContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                
                 return View(await _context.Items.ToListAsync());
            }
            else return RedirectToAction("Login", "users");

        }




        // GET: users/Create
        public IActionResult Create()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                var listOfusersId = _context.userRoles.Where(r => r.roleID == 2).ToList();

                ItemOperation itemOperation = new ItemOperation();
                List<User> managers = new List<User>();
                foreach(UserRole r in listOfusersId)
                {
                    var u = _context.users.Single(e => e.ID == r.userID);
                    managers.Add(u);

                }
                    // var manager = _context.users.Single(e => e.ID == listOfusersId[0].userID);
                itemOperation.managers = managers;


                List<Category> categories = _context.Category.ToList();
                itemOperation.categories = categories;
                
                // var ts = _context.users.Where(r => ID.Contains(r.RoleId));

                return View(itemOperation);
            }
            else return RedirectToAction("Login", "users");

        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ItemOperation itemOperation)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        itemOperation.item.IsActive = false;
                        _context.Items.Add(itemOperation.item);

                        await _context.SaveChangesAsync();

                         CategoryItem r = new CategoryItem { CategoryID = itemOperation.CategoryID, ItemID = itemOperation.item.ID };
                         _context.CategoryItem.Add(r);


                        await _context.SaveChangesAsync();

                         UserItem user = new UserItem { UserID = itemOperation.ManagerID, ItemID = itemOperation.item.ID };

                         _context.UserItem.Add(user);

                         await _context.SaveChangesAsync();



                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateException  ex )
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator."+"Be sure to not repeat Serial Number ");
                    //System.Threading.Thread.Sleep(5000);
                    return RedirectToAction("Create","Devices");////
                }
                return View(itemOperation);
            }
            else return RedirectToAction("Login", "users");
        }






        // GET: users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var item = await _context.Items
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (item == null)
                {
                    return NotFound();
                }


                return View(item);
            }
            else return RedirectToAction("Login", "users");
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                var item = await _context.Items.FindAsync(id);
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction("Login", "users");
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(i=> i.ID == id);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
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




        public async Task<IActionResult> Edit(int? id)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }



                ItemOperation itemOperation = new ItemOperation();
                CategoryItem r = _context.CategoryItem.Single(e => e.ItemID == item.ID);
                
                itemOperation.CategoryItem = r;

                List<Category> categories = _context.Category.ToList();
                itemOperation.categories = categories;
                itemOperation.item = item;

                return View(itemOperation);
            }
            else return RedirectToAction("Login", "users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemOperation itemOperation)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {


                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(itemOperation.item);
                        await _context.SaveChangesAsync();
                        //CategoryItem r = new CategoryItem { ID=itemOperation.CategoryItem.ID,CategoryID = itemOperation.CategoryItem.CategoryID, ItemID = itemOperation.item.ID };

                        _context.CategoryItem.Update(itemOperation.CategoryItem);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ItemExists(itemOperation.item.ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(itemOperation);
            }
            else return RedirectToAction("Login", "users");
        }



    }
}
