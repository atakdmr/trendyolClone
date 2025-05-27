using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using trendyolClone.Models;

namespace trendyolClone.Controllers
{
    //[Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly DataContext _context;

        public CategoryController(ILogger<CategoryController> logger, DataContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CategoryList(string q)
        {
            var categories = _context.Categories.Select(c => new Category
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    Url = c.Url,
                    Icon = c.Icon,
                    ProductCount = c.Products != null ? c.Products.Count() : 0 // Ürün sayısını hesapla
                })
                .ToList();

            if (!string.IsNullOrEmpty(q))
            {
                categories = categories.Where(c => c.CategoryName.ToLower().Contains(q.ToLower())).ToList();
            }

            return View(categories);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category c)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(c.Icon))
                {
                    ModelState.AddModelError("Icon", "Icon field is required.");
                    return View(c);
                }

                var categories = new Category
                {
                    CategoryName = c.CategoryName,
                    Url = c.Url,
                    Icon = c.Icon,
                    ProductCount = c.ProductCount,
                    Id = c.Id
                };
                _context.Categories.Add(categories);
                _context.SaveChanges();
                return RedirectToAction("CategoryList", "Category");
            }
            return View(c);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var categories = _context.Categories.Find(id);
            if(categories == null){
                return RedirectToAction("Index");
            }
            _context.SaveChanges();
            return View(categories);
        }
        [HttpPost]
        public IActionResult Details(Category c)
        {
                var categories = _context.Categories.Find(c.Id);
                if (categories != null)
                {
                    categories.CategoryName = c.CategoryName;
                    categories.Url = c.Url;
                    categories.Icon = c.Icon;
                    _context.SaveChanges();
                }
                return RedirectToAction("CategoryList", "Category");
        }
        public IActionResult Delete(int id)
        {
            var categories = _context.Categories.Find(id);
            if (categories != null)
            {
                _context.Categories.Remove(categories);
                _context.SaveChanges();
            }
            return RedirectToAction("CategoryList", "Category");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}