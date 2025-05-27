using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using trendyolClone.Models;

namespace trendyolClone.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _context;

    public HomeController(ILogger<HomeController> logger, DataContext context)
    {
        _context = context;
        _logger = logger;
    }
    public IActionResult Index()
    {
        var products = _context.Products.Where(p => p.HomePage).ToList();
        ViewData["Stock"] = _context.Products.Where(p => p.Stock).ToList();
        ViewData["ShowEverything"] = _context.Products.ToList();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Details(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["SimilarProducts"] = _context.Products.Where(p => p.CategoryId == product.CategoryId && p.Id != id).ToList();
        return View(product);
    }
    public IActionResult ProductList(string url, string q)
    {
        var products = _context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(url))
        {
            products = products.Where(p => p.Category.Url == url);
        }
        
        if (!string.IsNullOrEmpty(q))
        {
            products = products.Where(p => (p.Name != null && p.Name.ToLower().Contains(q.ToLower())) || (p.Description != null && p.Description.ToLower().Contains(q.ToLower())));
        }

        return View(products.ToList()); // Eksik dönüş eklendi
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
