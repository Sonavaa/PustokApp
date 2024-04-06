using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;

namespace PustokApp.Controllers
{
    public class ShopController : Controller
    {
		private readonly AppDbContext _context;
		public ShopController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
			var products = _context.Products
				 .Include(p => p.ProductImages)
				 .Include(p => p.Category)
				 .ToList();
			return View(products);
		}

        public IActionResult Details()
        {
			var products = _context.Products
				 .Include(p => p.ProductImages)
				 .Include(p => p.Category)
				 .ToList();
			return View(products);
		}
    }
}
