using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Extensions;
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
            ViewBag.Categories = await _context.Categories.ToListAsync();
            List<Product> products = await _context.Products.Include(x => x.ProductImages)
                                                  .Include(x => x.Category)
                                                  .Where(x => !x.IsDeleted)
                                                  .OrderByDescending(x => x.Id).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Author = await _context.Authors.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Author = await _context.Authors.ToListAsync();

            if (_context.Products.Any(x => x.Name == product.Name))
            {
                ModelState.AddModelError("", "Product already exists");
                return View(product);
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }
            if (product.File != null)
            {
                foreach (var file in product.File)
                {

                    if (!file.CheckFileType("image"))
                    {
                        ModelState.AddModelError("", "File Must Be An Image!");
                        return View(product);
                    }

                    if (!file.CheckFileSize(10))
                    {
                        ModelState.AddModelError("", "File Must Be Less Than 10MB!");
                        return View(product);
                    }

                    string uniqueFileName = await file.SaveFileAsync(_env.WebRootPath, "Client", "assets", "image", "products");

                    var additionalProductImages = CreateProduct(_env.WebRootPath,false,false,product);

                    product.ProductImages.Add(additionalProductImages);

                }
            }


            if (!product.MainFile.CheckFileType("image"))
            {
                ModelState.AddModelError("MainFile", "File Must Be An Image!");
                return View(product);
            }

            if (!product.MainFile.CheckFileSize(10))
            {
                ModelState.AddModelError("MainFile", "File Must Be Less Than 10MB!");
                return View(product);
            }

            string mainFileName = await product.MainFile.SaveFileAsync(_env.WebRootPath, "Client", "assets", "image", "products");

            var mainProductImageCreate = CreateProduct(mainFileName, false, true, product);

            product.ProductImages.Add(mainProductImageCreate);


            if (!product.HoverFile.CheckFileType("image"))
            {
                ModelState.AddModelError("HoverFile", "File Must Be An Image!");
                return View(product);
            }

            if (!product.MainFile.CheckFileSize(10))
            {
                ModelState.AddModelError("HoverFile", "File Must Be Less Than 10MB!");
                return View(product);
            }

            string hoverFileName = await product.HoverFile.SaveFileAsync(_env.WebRootPath, "Client", "assets", "image", "products");

            var hoverProductImageCreate = CreateProduct(hoverFileName, true, false, product);
            product.ProductImages.Add(hoverProductImageCreate);


            ProductImage CreateProduct(string url, bool isHover, bool isMain, Product product)
            {
                return new ProductImage
                {
                    Url = url,
                    IsMain = isMain,
                    IsHover = isHover,
                    Product = product
                };
            }

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

   

        // Butun Crudlarda silinen adlari yeniden yaratmaq olmur yeniden yaratmaq ucun isdeleted false sertini ver
        //nomrelemeni orderbydescending ele



        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
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

