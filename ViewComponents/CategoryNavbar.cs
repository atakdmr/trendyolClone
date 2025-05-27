using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using trendyolClone.Models;

namespace trendyolClone.ViewComponents
{
    public class CategoryNavbar : ViewComponent
    {
        private readonly DataContext _context;
        public CategoryNavbar(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
    }
}