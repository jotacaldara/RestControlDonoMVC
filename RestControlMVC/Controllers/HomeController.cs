using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.Models;

namespace RestControlMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: /Home/Features
        public IActionResult Features()
        {
            return View();
        }

        // GET: /Home/Pricing
        public IActionResult Pricing()
        {
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // Redirecionar para Auth/Login
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Auth");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
