using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace trendyolClone.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? Icon { get; set; }
        public int ProductCount { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}