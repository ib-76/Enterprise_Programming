using Common.Models;
using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Models;


namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {

        private CategoriesRepository  myCategoriesRepository;
        private ProductsRepository  myProductsRepository;
        public ProductsController(CategoriesRepository efficientCategoriesRepository , ProductsRepository productsRepository ) //requesting an instance of Categories Repository via constructor injection
        {
            myCategoriesRepository = efficientCategoriesRepository;
            myProductsRepository = productsRepository;
        }

        public IActionResult Index(int page = 1, int pageSize = 6)
        {//the term models is used for object types that transport data to/from views to/from the controller
            var list = myProductsRepository.Get().Skip((page-1)*pageSize).Take(pageSize); 
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemsFetched = list.Count();
            ViewBag.PageSize = pageSize;


            return View(list);
        }


        //roles of a
        //controller:- business logic related to the UI while the
        //repository:- handles database operations

        // actions that can be coded in a controller BUT NOT in a repository
        //1. request> data
        //2. parse the data
        //3. sanitise/validate data
        //4. save /fetch data from the db
        //5. reshape /transform data in such a way that is suitable for the user to understand it
        //6. decide where to redirect the user
        //7.response < data

        [HttpGet]
        public IActionResult Create()
        {
            var myPreparedSqlofCategories =myCategoriesRepository.GetAllCategories();

            ProductsCreateViewModel myModel= new ProductsCreateViewModel ();
            myModel.Categories = myPreparedSqlofCategories.ToList();


            return View(myModel); //mvc will return a view from the folder VIEW which shpuld have a subfolder named Products and inside it a view named Create.cshtml
        }

        [HttpPost]
        public IActionResult Submit(ProductsCreateViewModel p){ // model binding in action ...method injecting the repository via method injection
        try
        {
            //add the product keyed in by the user to the db NOTE NO LINQ code here
            myProductsRepository.Add(p.Product);
            TempData["success"] = "Product added successfully!";
            return View("Index"); // this redirect was necessay because the method name is Submit not Create  .. server-side redirection
            //return RedirectToAxction("index")           // client-side redirection ....viewbag wont work but tempdata will work
            }
            catch (Exception ex)
        {
        TempData["error"] = "Product failed to be added";
            return View("Create");
            }


        // note: ways of passing data from view => controller
        //1.Parameters  
        //2. FormCollection
        //3. Model Binding
}
        [HttpPost]
        public IActionResult Search (string keyword)
        {
            // note : ways of passing data from controller => view
            //1. Viewbag ; dynamic object,whatever i store inside doesnn't survive redirect
            //2. TemporaryData, survives redirect (only for one request)
            //3. Model
            //4. cookies
            //5. session varaiables

            // search in the db for products matching the keyword




            //Notes on defferre executioon ( ie using the IQueryble)
            // get() => 1st call
            // where() => 2nd call
            // OrderBy() +> 3rd call

            //because of IQueryable()
            //after 1st call => Select * From Products
            //after 2ndt call = Select * From Products Where Name likje '%keyword5' or Description Like '%description%'
            //after 3rd call = Select * From Products Where Name likje '%keyword5' or Description Like '%description%' OrderBy Name 
           var List = myProductsRepository.Get().Where(p => p.Name.Contains(keyword)
                                        || p.Description.Contains(keyword))
                .OrderBy(p => p.Name).ToList();
            

            // you control where the user is redirected after the method is executed
            // by default it will look for a view with the same name as the method when you use return View(); ie products/search.cshtml
            // tp redirect to another action method use return View("name of the view");
            return View("Index" , List );
            
        }
       
    }
}
