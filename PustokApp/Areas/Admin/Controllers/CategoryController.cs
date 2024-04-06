using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;

namespace NestApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {

            if (ModelState["Name"] == null) {
                return View(category);
            }

            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Name Is Required!");
                return View(category);
            }

            Category newCategory = new Category
            {
                Name = category.Name,
                
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {





            return View();
        }
    }
}