using Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{


    // Note:  context class is an abstarction of the database - it manages the entity objects during runtime, which includes populating objects with data from a database, change tracking, and persisting data to the database.
    // It alllows you to implement certain configurations such:-
    // - changing the connection string ( event though it is not recommended) will point the context to a different database
    // - enable lazy loading 
    // - adding constraints and relationships 

    // identifyDBContext as derived from DbContext : DbContext but you get an  upgrade - it creates the tables that allows us to manage user sessions and identity management

    public class ShoppingCartDbContext : IdentityDbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } // Products is the table name in the database

        public DbSet<Category> Categories { get; set; } // Categories is the table name in the database

        public DbSet<Order> Orders { get; set; } // Orders is the table name in the database
        public DbSet<OrderItem> OrderItems { get; set; } // OrderItems is the table name in the database


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //lazy loading => loads the data in the nacigational properties automatically

            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }





    }
}


