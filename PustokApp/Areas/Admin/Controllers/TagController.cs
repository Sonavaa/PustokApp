using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;

namespace PustokApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags.Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).ToListAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _context.Tags.Where(x => x.Name == null).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            var tags = await _context.Tags.Where(x => x.Name == null).ToListAsync();

            if (ModelState["Name"] == null)
            {
                return View(tags);
            }

            var isExistTag = await _context.Tags.AnyAsync(x => x.Name.ToLower() == tag.Name.ToLower());

            if (isExistTag)
            {
                ModelState.AddModelError("Name", "Tag Already Exist!");
                return View(tags);
            }

            Tag newTags = new Tag
            {
                Name = tag.Name,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CreatedBy = "Admin"
            };

            await _context.Tags.AddAsync(newTags);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var tags = _context.Tags.Where(x => x.Name == null && x.IsDeleted == false).FirstOrDefault(x => x.Id == id);

            if (id == null || id == 0)
            {
                return View();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, Tag tag)
        {
            var tags = _context.Tags.FirstOrDefault(x => x.Id == id);

            var isExist = _context.Tags.Any(x => x.Name.ToLower() == tag.Name.ToLower() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "This tag alredy exist");
                return View(tag);
            }

            tags.Name = tag.Name;
            tags.UpdatedAt = DateTime.UtcNow.AddHours(4);
            tags.UpdatedBy = "Admin";


            if (tags.Name == null)
            {

                return RedirectToAction("Update", new { id = id });
            }

            _context.Update(tags);
            _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var tags = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tags == null)
            {
                return NotFound();
            }
            else
            {
                tags.IsDeleted = true;
                tags.DeletedAt = DateTime.UtcNow.AddHours(4);
                tags.DeletedBy = "Admin";
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}


