using Common.Models;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoriesRepository
    {

        private ShoppingCartDbContext myContext;
        public  CategoriesRepository(ShoppingCartDbContext myEffitientContext)
        {
            myContext = myEffitientContext;
        }

        

        // Reads the entire list of categories from the database table Categories
        public IQueryable<Category> GetAllCategories()
        {
           
            return myContext.Categories.OrderBy(c => c.Order); ;
        }


        //.....explaning IQueryable vs List
        public IQueryable<Category> GetSortedCategories()
        {
            //when using  IQueryable instead of List 
            //1. example below SQL string :" Select * from Categories Order by [Order] " opens db connnection twice when using ToList() or foreach
            //2. prepare an sql query is prepared but no execution yet and db still not opened 
            
            return GetAllCategories().OrderBy(c => c.Order);
        }
        //.....end explaning IQueryable vs List

        public void Add(Category category)
        {
            if (category.Order >= 0)
            {
                myContext.Categories.Add(category); 
                myContext.SaveChanges();  
            }
        }


        public void Delete(Category category)
        {
            myContext.Categories.Remove(category);
            myContext.SaveChanges();

        }


        public Category ? Get(int id) 
        {
           
            return myContext.Categories.SingleOrDefault(p => p.Id == id); 

        }

        public void Delete(int id)
        {
            var categorytoDelete = Get(id);
            if (categorytoDelete != null)
            {
                //ShoppingCartDbContext myContext = new ShoppingCartDbContext(null);
                myContext.Categories.Remove(categorytoDelete);
                myContext.SaveChanges();
            }

        }
    }

}