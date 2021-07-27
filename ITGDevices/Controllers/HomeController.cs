using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ITGDevices.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Employee()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Employee", true) == 0)
                return RedirectToAction("Index", "DevicesRequest");
            else return RedirectToAction("Login", "users");
        }
        public IActionResult OperationsManager()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "OperationsManager", true) == 0)
            {
                return RedirectToAction("Teacher", "users");

            }
            else return RedirectToAction("Login", "users");

        }
        public IActionResult Admin()
        {
            if (string.Compare(HttpContext.Session.GetString("role"), "Admin", true) == 0)
                return RedirectToAction("Index", "users");
            else return RedirectToAction("Login", "users");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
