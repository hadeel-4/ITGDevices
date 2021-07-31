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
        public async Task<IActionResult> RequestDevice(ItemOperation obj)
        {
            if ((string.Compare(HttpContext.Session.GetString("role"), "Employee", true) == 0))
            {
                var item = await _context.Items
                   .FirstOrDefaultAsync(m => m.ID == obj.item.ID);//
                if (item == null)
                {
                    return NotFound();
                }


                
                UserItem h = _context.UserItem.Single(i => i.ItemID == item.ID);
                 User holder = _context.users.Single(i => i.ID == h.UserID);

                

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
                mail.To.Add(new MailAddress(holder.Email));

               
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                StreamReader reader = new StreamReader($"{Directory.GetCurrentDirectory()}/wwwroot/files/Body.cshtml");
              string readFile = reader.ReadToEnd();


                // mail.Body = readFile;
                UserItemRequest userItemRequest = new UserItemRequest { ItemID = item.ID, UserID = (int)HttpContext.Session.GetInt32("id") };
                 _context.UserItemRequest.Add(userItemRequest);
                await _context.SaveChangesAsync();

                var itemuser = await _context.UserItemRequest
                   .FirstOrDefaultAsync(m => m.ItemID ==item.ID && m.UserID== (int)HttpContext.Session.GetInt32("id"));//
                if (itemuser == null)
                {
                    return NotFound();
                }
                mail.Body = "Device Requested" +"<br/>"+
                 "<a href ="+ "https://localhost:44367/DevicesRequest/AcceptOrReject?id="+itemuser.ID+ ">" +
                      "Approve/Reject"+"</a> ";

                        
                mail.IsBodyHtml = true;
                mail.Subject = "Devices";

                try
                {
                    smtp.Send(mail);
                    
                   // HttpContext.Session.SetInt32("holderId", holder.ID);

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








        public async Task<IActionResult> Accept(ItemOperation obj)
        {
            if ((string.Compare(HttpContext.Session.GetString("role"), "Employee", true) == 0)|| (string.Compare(HttpContext.Session.GetString("role"), "OperationsManager", true) == 0))
            {
                if (obj == null)
                {
                    return NotFound();
                }
                var item = await _context.Items
                   .FirstOrDefaultAsync(m => m.ID == obj.item.ID);//
                if (item == null)
                {
                    return NotFound();
                }
                if (!item.IsDeliver)
                {
                    
                   
                }



                UserItem h = _context.UserItem.Single(i => i.ItemID == item.ID);
                User holder = _context.users.Single(i => i.ID == h.UserID);



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
                mail.To.Add(new MailAddress(holder.Email));


                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                StreamReader reader = new StreamReader($"{Directory.GetCurrentDirectory()}/wwwroot/files/Body.cshtml");
                string readFile = reader.ReadToEnd();


                // mail.Body = readFile;
                UserItemRequest userItemRequest = new UserItemRequest { ItemID = item.ID, UserID = (int)HttpContext.Session.GetInt32("id") };
                _context.UserItemRequest.Add(userItemRequest);
                await _context.SaveChangesAsync();

                var itemuser = await _context.UserItemRequest
                   .FirstOrDefaultAsync(m => m.ID==userItemRequest.ID);//
                if (itemuser == null)
                {
                    return NotFound();
                }
                mail.Body = "Device Requested" + "<br/>" +
                    "<a href='https://localhost:44367/DevicesRequest/AcceptOrReject?id=" + itemuser.ID.ToString() + " '>Approve/Reject</a>";
               


                mail.IsBodyHtml = true;
                mail.Subject = "Devices";

                try
                {
                    smtp.Send(mail);

                    // HttpContext.Session.SetInt32("holderId", holder.ID);

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
                        var RoleInfo = _context.roles.Single(e => e.ID == role.roleID);
                        HttpContext.Session.SetString("role", RoleInfo.rolename);
                        HttpContext.Session.SetInt32("idd", User1.ID);


                        return RedirectToAction("AcceptOrReject", "DevicesRequest");
                        //return View("Index");

                    }
                }

            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to LOGIN." + ex.Message);
            }
            return View();
        }


        public async Task<IActionResult> AcceptOrReject(System.Guid id)
        {
            
            if ((string.Compare(HttpContext.Session.GetString("role"), "Employee", true) == 0) || (string.Compare(HttpContext.Session.GetString("role"), "OperationsManager", true) == 0))
            {
                

                var UserItemRequest = await _context.UserItemRequest.FindAsync(id);
                if (UserItemRequest == null)
                {
                    ModelState.AddModelError("", "UserItemRequest is null");



                    return NotFound();
                }
                var userItem = await _context.UserItem.FindAsync(UserItemRequest.ItemID);
                if (userItem == null)
                {
                    ModelState.AddModelError("", "userItem is null");

                    return NotFound();
                }
                var Realholder = await _context.users.FindAsync(userItem.UserID);
                if (Realholder == null)
                {
                    return NotFound();
                }
                var item = await _context.Items.FindAsync(UserItemRequest.ItemID);
                if (item == null)
                {
                    return NotFound();
                }
                var Requester = await _context.users.FindAsync(UserItemRequest.UserID);
                if (Requester == null)
                {
                    return NotFound();
                }





                if ((int)HttpContext.Session.GetInt32("idd")== Realholder.ID)
                {
                    ItemOperation itemOperation = new ItemOperation();
                    itemOperation.item = item;
                    itemOperation.holder = Realholder;
                    itemOperation.requester = Requester;
                    itemOperation.UserItemRequest = UserItemRequest;
                    System.Diagnostics.Debug.WriteLine("done");


                    return View(itemOperation);
                }
                 else return RedirectToAction("Login", "DevicesRequest",id);
            }
            else return RedirectToAction("Login", "DevicesRequest",id);

        }

    }
}
