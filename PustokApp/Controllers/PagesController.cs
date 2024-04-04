using Microsoft.AspNetCore.Mvc;

namespace PustokApp.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Compare()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            return View();
        }

        public IActionResult LoginRegister()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult OrderComplate()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }
    }
}
