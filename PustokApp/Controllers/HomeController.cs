using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using System.Diagnostics;

namespace PustokApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products= _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .ToList();
            return View(products);
        }

    }
}
