using ITGDevices.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGDevices.Models;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult RequestDevice(ItemOperation obj)
        {
            if ((string.Compare(HttpContext.Session.GetString("role"), "Employee", true) == 0))
            {


                /* var item = obj.item;


                         var listOfusersId = _context.userRoles.Where(r => r.roleID == 2).ToList();
                         List<User> managers = new List<User>();
                         foreach (UserRole r in listOfusersId)
                         {
                             var u = _context.users.Single(e => e.ID == r.userID);
                             managers.Add(u);

                         }
                         User manager = managers[0];
                         int uid = (int)HttpContext.Session.GetInt32("id");
                         User caurrentUser = _context.users.Single(e => e.ID == uid);
   */
               // UserItem h = _context.UserItem.Single(i => i.ItemID == item.ID);
                // User holder = _context.users.Single(i => i.ID == h.UserID);
                // ItemOperation itemOperation = new ItemOperation();
                //itemOperation.item = item;
                // itemOperation.holder = holder;

                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress("ha412233@gmail.com");
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mail.From.Address, "hadeel 123456789");

                smtp.Host = "smtp.gmail.com";

                //recipient
                mail.To.Add(new MailAddress("ha986562@gmail.com"));

                mail.IsBodyHtml = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                StreamReader reader = new StreamReader($"{Directory.GetCurrentDirectory()}/wwwroot/files/Body.cshtml");
                string readFile = reader.ReadToEnd();

                
                 mail.Body = readFile;
                mail.Subject = "Devices";

                try
                {
                    smtp.Send(mail);
                    System.Diagnostics.Debug.WriteLine("done");
                }
                catch (SmtpException ex)
                {

                    Console.WriteLine(ex.StackTrace);
                }
               
                
                


                return RedirectToAction("Index", "DevicesRequest");
            }
            else return RedirectToAction("Login", "users");
        }
    }
}
