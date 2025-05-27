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
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DataContext _context;

        public AdminController(ILogger<AdminController> logger, DataContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.Take(5).Select(c => new Category
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Url = c.Url,
                ProductCount = c.Products.Count(),
                Icon = c.Icon
            }).ToList();
            return View(categories);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}