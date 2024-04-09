using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Extensions;
using PustokApp.Models;

namespace NestApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.Where(x => x.ParentId == null).ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var categories = await _context.Categories.Where(x => !x.IsDeleted && x.ParentId == null).ToListAsync();
            ViewBag.Categories = categories;

            if (ModelState["Name"] == null || ModelState["ParentId"] == null)
            {
                return View(category);
            }

            if (category.ParentId != null)
            {
                var isExistSubcategory = await _context.Categories.AnyAsync(x => x.Id == category.ParentId && x.ParentId == null);

                if (!isExistSubcategory)
                {
                    ModelState.AddModelError("ParentId", "This Category Is Not Found!");
                    return View(category);
                }
            }

            var isExistCategory = await _context.Categories.AnyAsync(x => x.Name.ToLower() == category.Name.ToLower());

            if (isExistCategory)
            {
                ModelState.AddModelError("Name", "Category Already Exist!");
                return View(category);
            }

            Category newCategory = new Category
            {
                Name = category.Name,
                ParentId = category.ParentId,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CreatedBy = "Admin"
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var categories = _context.Categories.Where(x => x.ParentId == null && x.IsDeleted == false).ToList();
            Category? existsCategory = _context.Categories.FirstOrDefault(s => s.Id == id);
            ViewBag.Categories = categories;

            if (id == null || id == 0)
            {
                return View();
            }

            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            //if (ModelState["Name"] == null)
            //{
            //    return BadRequest();
            //}

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            var categories = await _context.Categories.Where(x => x.ParentId == null).ToListAsync();
            Category? existsCategory = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            ViewBag.Categories = categories;


            if (category.ParentId is not null)
            {
                var isExistSubcategory = await _context.Categories.AnyAsync(x => x.Id == category.ParentId && x.ParentId == null);

                if (!isExistSubcategory)
                {
                    ModelState.AddModelError("ParentId", "This Category Is Not Found!");
                    return View(category);
                }
            }


            var isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == category.Name.ToLower() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "Name alredy exist");
                return View(category);
            }


            if (existsCategory != null)
            {
                existsCategory.ParentId = category.ParentId;
                existsCategory.Name = category.Name;
                existsCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
                existsCategory.UpdatedBy = "Admin";
            }
            else
            {
                return View(category);
            }

            if (category.Name == null)
            {

                return RedirectToAction("Update", new { id = id });
            }

            _context.Update(existsCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var categories = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (categories is null)
            {
                return NotFound();
            }
            else
            {
                categories.IsDeleted = true;
                categories.DeletedAt = DateTime.UtcNow.AddHours(4);
                categories.DeletedBy = "Admin";
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}