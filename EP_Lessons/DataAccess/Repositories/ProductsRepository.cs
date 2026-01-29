using Common.Models;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductsRepository
    {

        // Dependency Injection  of the context object via the constructor
        // this allows the use of am instance of an object which we have to assume its been created in the beginning of the application lifecycle
        // how can we requesr to useit?
        // - (most populart) via constructor injection
        // - via method injection
        // - via property injection

        // ProductsRepository class is the client class that depends on the
        // ShoppingCartDbContext class that is the service class

        public ProductsRepository(ShoppingCartDbContext myEffitientContext)
        {
            myContext = myEffitientContext;
        }

        private ShoppingCartDbContext myContext;

        public void Add(Product product)
        {
            if (product.Stock >= 0)
            {
                //add product to database

                //ShoppingCartDbContext myContext = new ShoppingCartDbContext(null);
                myContext.Products.Add(product); // this add the product object  into  a Product List the context opject ie in volitile memory

                //when SaveChanges is called  the context object  will generate the appropriate SQL commands to insert the new product into the database
                myContext.SaveChanges();  //  saves permanenty the changes contained in mycontext into the database
            }
        }


        public void Delete(Product product)
        {
            //ShoppingCartDbContext myContext = new ShoppingCartDbContext(null);
            myContext.Products.Remove(product);
            myContext.SaveChanges();

        }


        public Product? Get(int id) // ? means it can return null
        {
            //ShoppingCartDbContext myContext = new ShoppingCartDbContext(null);
            return myContext.Products.SingleOrDefault(p => p.Id == id); // lambda expression replacing a foreach loop

        }

        public void Delete(int id)
        {
            var productToDelete = Get(id);
            if (productToDelete != null)
            {
                //ShoppingCartDbContext myContext = new ShoppingCartDbContext(null);
                myContext.Products.Remove(productToDelete);
                myContext.SaveChanges();
            }

        }


        public IQueryable<Product> Get()
        {
            return myContext.Products; // Preparing but not executing Select * From Products
        }

    }
}
//ShoppingCartDbContext myContext = new ShoppingCartDbContext(null);
// this is inefficient coding because a new context is created each time for each method call
// better approach is to create a single context object and reuse it for all method calls 
// by creting  private ShoppingCartDbContext myEffitientContext and initializing it in the constructor
// public ProductsRepositories() { myEffitientContext = new ShoppingCartDbContext(null);  }
// and then using myEffitientContext in all methods