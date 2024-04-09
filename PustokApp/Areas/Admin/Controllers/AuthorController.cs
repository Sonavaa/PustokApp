using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;

namespace PustokApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;
        public AuthorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).ToListAsync();
            return View(authors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var authors = await _context.Authors.Where(x => x.Name == null).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            var authors = await _context.Authors.Where(x => x.Name == null).ToListAsync();

            if (ModelState["Name"] == null)
            {
                return View(authors);
            }

            var isExistAuthor = await _context.Authors.AnyAsync(x => x.Name.ToLower() == author.Name.ToLower());

            if (isExistAuthor)
            {
                ModelState.AddModelError("Name", "Author Already Exist!");
                return View(authors);
            }

            Author newAuthor = new Author
            {
                Name = author.Name,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CreatedBy = "Admin"
            };

            await _context.Authors.AddAsync(newAuthor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var authors = _context.Authors.Where(x => x.Name == null && x.IsDeleted == false).FirstOrDefault(x => x.Id == id);

            if (id == null || id == 0)
            {
                return View();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Update(int id,Author author)
        {
            var authors = _context.Authors.FirstOrDefault(x => x.Id == id);

            var isExist = _context.Authors.Any(x => x.Name.ToLower() == author.Name.ToLower() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "Name alredy exist");
                return View(author);
            }

            authors.Name = author.Name;
            authors.UpdatedAt = DateTime.UtcNow.AddHours(4);
            authors.UpdatedBy = "Admin";
            

            if (authors.Name == null)
            {

                return RedirectToAction("Update", new { id = id });
            }

            _context.Update(authors);
            _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var authors = _context.Authors.FirstOrDefault(x => x.Id == id);
            if (authors == null)
            {
                return NotFound();
            }
            else
            {
                authors.IsDeleted = true;
                authors.DeletedAt = DateTime.UtcNow.AddHours(4);
                authors.DeletedBy = "Admin";
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
