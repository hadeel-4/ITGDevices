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
    public class CategoryController : Controller
    {
        private readonly DeviceContext _context;


        public CategoryController(DeviceContext context)
        {
            _context = context;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
                return View(await _context.Category.ToListAsync());
            else return RedirectToAction("Login", "users");

        }




        // GET: users/Create
        public IActionResult Create()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                
                return View();
            }
            else return RedirectToAction("Login", "users");

        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(Category category)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(category);

                        await _context.SaveChangesAsync();
                       

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
                return View(category);
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

                var category = await _context.Category
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (category == null)
                {
                    return NotFound();
                }


                return View(category);
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
                var category = await _context.Category.FindAsync(id);
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction("Login", "users");
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.Category
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (category == null)
                {
                    return NotFound();
                }
               
                return View(category);

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

                var category = await _context.Category.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            else return RedirectToAction("Login", "users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {


                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(category);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(category.ID))
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
                return View(category);
            }
            else return RedirectToAction("Login", "users");
        }


        private bool CategoryExists(int id)
        {
            return _context.Category.Any(category => category.ID == id);
        }
    }
}
