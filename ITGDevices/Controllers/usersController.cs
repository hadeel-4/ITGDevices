using ITGDevices.Data;
using ITGDevices.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ITGDevices.Controllers
{
    public class usersController : Controller
    {

        private readonly DeviceContext _context;


        public usersController(DeviceContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View(new Models.User());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login user)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    var User1 = _context.users.Single(e => e.username == user.username && e.Password == user.Password);
                    if (User1 != null)
                    {
                        var role = _context.userRoles.Single(e => e.userID == User1.ID);
                        // var t = _context.userRoles.Single(e => e.roleID == r.roleID);
                        var RoleInfo = _context.roles.Single(e => e.ID == role.roleID);
                        HttpContext.Session.SetString("role", RoleInfo.rolename);
                        HttpContext.Session.SetString("role2", "in");
                        HttpContext.Session.SetString("username", user.username);
                        HttpContext.Session.SetString("firstname", User1.FirstName);
                        HttpContext.Session.SetInt32("id", User1.ID);

                        return RedirectToAction(HttpContext.Session.GetString("role"), "Home");
                        //return View("Index");

                    }
                }

            }
            catch (Exception  ex )
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to LOGIN."+ex.Message);
            }
            return View();
        }
        // GET: users
        public async Task<IActionResult> Index()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
                return View(await _context.users.ToListAsync());
            else return RedirectToAction("Login", "users");

        }




        // GET: users/Create
        public IActionResult Create()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                UsersRoles ur = new UsersRoles();
                ur.roles = _context.roles.ToList();
                return View(ur);
            }
            else return RedirectToAction("Login", "users");

        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(UsersRoles ur)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(ur.user);

                        await _context.SaveChangesAsync();
                        UserRole r = new UserRole { userID = ur.user.ID, roleID = ur.roleId };
                        _context.userRoles.Add(r);


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
                return View(ur);
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

                var user = await _context.users
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (user == null)
                {
                    return NotFound();
                }


                return View(user);
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
                var user = await _context.users.FindAsync(id);
                _context.users.Remove(user);
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

                var user = await _context.users
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (user == null)
                {
                    return NotFound();
                }
                var role = _context.userRoles.Single(e => e.userID == user.ID);
                var RoleInfo = _context.roles.Single(e => e.ID == role.roleID);
                List<Role> roles = new List<Role>();
                roles.Add(RoleInfo);

                UsersRoles ur = new UsersRoles();
                ur.user= user;
                ur.roles = roles;
                return View(ur);

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

                var user = await _context.users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            else return RedirectToAction("Login", "users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
            {
                

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.ID))
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
                return View(user);
            }
            else return RedirectToAction("Login", "users");
        }


        private bool UserExists(int id)
        {
            return _context.users.Any(user => user.ID == id);
        }
















        public IActionResult Logout()
        {
           
            HttpContext.Session.SetString("role", "out");
            /////
            HttpContext.Session.SetString("role2", "out");
            return RedirectToAction("Login", "users");
        }
    }
}
