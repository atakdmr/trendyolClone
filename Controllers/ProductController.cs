using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using trendyolClone.Models;

namespace trendyolClone.Controllers
{
    //[Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(ILogger<ProductController> logger, DataContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            this._context = context;
            this._env = env;
        }

        public IActionResult Index()
        {
            ViewBag.Category = new SelectList(_context.Categories, "Id", "CategoryName");
            var products = _context.Products.Take(5).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product p, IFormFile Image)
        {
            var product = new Product
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                HomePage = p.HomePage,
                CategoryId = p.CategoryId,
                       
                
            };
            if (Image != null && Image.Length > 0)
            {
                var ext = Path.GetExtension(Image.FileName).ToLower(); // .png/.jpg, dosyanın uzantısını al.

                var fileName = $"{Guid.NewGuid()}{ext}"; // random isim oluştur ve uzantısını al.
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads"); // yükleme klasörü oluştur.

                Directory.CreateDirectory(uploadPath); // eğer uploads diye bi klasör yoksa oluştur

                var filePath = Path.Combine(uploadPath, fileName); // dosya yolu belirle
                await using var stream = new FileStream(filePath, FileMode.Create); // dosyaya veri yazılabilir, okunulabilir ve işi bittiğinde sayfayı yormadan halledip bitirir işini.
                await Image.CopyToAsync(stream); // dosyayı kopyalama

                p.FilePath = "/uploads/" + fileName; // ürün nesnesine dosya yolu ekleme, yani p'nin imagei buraya gelcek diyoruz
            }
            await _context.Products.AddAsync(p);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> Details(Product p, int id, IFormFile Image)
        {
            if (id != p.Id) return NotFound();

            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null) return NotFound();

                //güncelle
                existingProduct.Name = p.Name;
                existingProduct.Description = p.Description;
                existingProduct.Price = p.Price;
                existingProduct.Stock = p.Stock;
                existingProduct.HomePage = p.HomePage; 
                existingProduct.CategoryId = p.CategoryId;
 
                if (Image != null && Image.Length > 0)
                {
                    var ext = Path.GetExtension(Image.FileName).ToLower();
                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

                    Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, fileName);
                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await Image.CopyToAsync(stream);

                    // Yeni resim yolu ayarla
                    existingProduct.FilePath = "/uploads/" + fileName;
                }
                _context.Update(existingProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return BadRequest("There is an error that we dont know. Try to restart the app.");
            }

        }
        public IActionResult Delete(int id)
        {
            var Product = _context.Products.Find(id);
            if (Product != null)
            {
                _context.Products.Remove(Product);
                _context.SaveChanges();
            }
            return RedirectToAction("ProductList", "Product");
        }
        public IActionResult ProductList()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}