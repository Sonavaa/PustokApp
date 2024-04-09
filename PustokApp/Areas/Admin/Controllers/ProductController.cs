using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;

namespace PustokApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(x => x.ProductImages)
                                                  .Include(x => x.Category)
                                                  .Where(x => !x.IsDeleted)
                                                  .OrderByDescending(x => x.Id).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _context.Products.Include(x => x.ProductImages)
                                                 .Include(x => x.Category)
                                                 .Include(x=>x.Author)
                                                 .Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            var products = await _context.Products.Where(x=> !x.IsDeleted).ToListAsync();
            ViewBag.Products = products;

            if (!ModelState.IsValid)
            {
                return View(products);
            }




            // Product Image Create Et Ve Check Et + isHover Column Yarat
            // Create Viewda Image ucun input yarat
            //Tagsi checkbox ele
            //product code avtomatik yarat

            // Butun Crudlarda silinen adlari yeniden yaratmaq olmur yeniden yaratmaq ucun isdeleted false sertini ver
            //nomrelemeni orderbydescending ele





            Product newProduct = new Product
            {
                Name = product.Name,
                Author = product.Author,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                ExTax = product.ExTax,
                ProductCode = product.ProductCode,
                RewardPoint = product.RewardPoint,
                Review = product.Review,
                Description = product.Description,
                StockCount = product.StockCount,
                Category = product.Category,
                //ProductImages = product.ProductImages,
                Tags = product.Tags,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CreatedBy = "Admin"
            };

            if (product.StockCount > 0)
            {
                product.IsAvailable = true;
            }

            var isExistProduct = await _context.Products.AnyAsync(x => x.Name.ToLower() == product.Name.ToLower());

            if (isExistProduct)
            {
                ModelState.AddModelError("Name", "Category Already Exist!");
                return View(product);
            }

            if (product.Category != null)
            {
                var HasBookCategory = await _context.Products.AnyAsync(x => x.Category == product.Category && x.Category == null);

                if (!HasBookCategory)
                {
                    ModelState.AddModelError("Category", "Book's Category Not Found!");
                    return View(product);
                }
            }

            if (product.Author != null)
            {
                var HasBookAuthor = await _context.Products.AnyAsync(x => x.Author == product.Author && x.Author == null);

                if (!HasBookAuthor)
                {
                    ModelState.AddModelError("Author", "Book's Author Not Found!");
                    return View(product);
                }
            }



            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }























        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                product.IsDeleted = true;
                product.DeletedAt = DateTime.UtcNow.AddHours(4);
                product.DeletedBy = "Admin";
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

