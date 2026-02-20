using Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    // CatalogueDbContext is the abstraction of the database for Bistro_Base
    // It manages Restaurants and MenuItems during runtime.
    // Allows change tracking, lazy loading, and relationships.
    public class CatalogueDbContext : IdentityDbContext
    {
        public CatalogueDbContext(DbContextOptions<CatalogueDbContext> options) : base(options)
        {
        }

        // Tables
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Enable lazy loading for navigation properties
            optionsBuilder.UseLazyLoadingProxies();
        }

     
    }
}
