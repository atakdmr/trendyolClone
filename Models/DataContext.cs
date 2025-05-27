using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace trendyolClone.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, CategoryName = "Sport", Url = "sport", Icon = "fa fa-running" },
                new Category { Id = 2, CategoryName = "Electronics", Url = "electronics", Icon = "fa fa-mobile-alt" },
                new Category { Id = 3, CategoryName = "Clothes", Url = "clothes", Icon = "fa fa-tshirt" },
                new Category { Id = 4, CategoryName = "Books", Url = "books", Icon = "fa fa-book" },
                new Category { Id = 5, CategoryName = "Toys", Url = "toys", Icon = "fa fa-dice" },
                new Category { Id = 6, CategoryName = "Food", Url = "food", Icon = "fa fa-utensils" },
                new Category { Id = 7, CategoryName = "Furniture", Url = "furniture", Icon = "fa fa-couch" },
                new Category { Id = 8, CategoryName = "Beauty", Url = "beauty", Icon = "fa fa-heart" },
                new Category { Id = 9, CategoryName = "Health", Url = "health", Icon = "fa fa-stethoscope" },
                new Category { Id = 10, CategoryName = "Automotive", Url = "automotive",Icon = "fa fa-car" }
                
            );
        }
    }
    
}