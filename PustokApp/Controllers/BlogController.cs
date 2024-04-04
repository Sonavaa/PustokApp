using Microsoft.AspNetCore.Mvc;

namespace PustokApp.Controllers
{
	public class BlogController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Details()
		{
			return View();
		}
	}
}
