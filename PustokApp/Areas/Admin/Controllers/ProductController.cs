using Microsoft.AspNetCore.Mvc;

namespace PustokApp.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
