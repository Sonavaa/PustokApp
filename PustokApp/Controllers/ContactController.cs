using Microsoft.AspNetCore.Mvc;

namespace PustokApp.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
