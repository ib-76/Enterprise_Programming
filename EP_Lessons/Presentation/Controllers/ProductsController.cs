using Common.Models;
using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {

        //  private ProductsRepository myProductsRepository;
        //  public ProductsController(ProductsRepository efficientProductsRepository) //requesting an instance of ProductsRepository via constructor injection
        //  {
        //      myProductsRepository = efficientProductsRepository;
        //   }


        public IActionResult Index()
        {
            return View();
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

            return View(); //mvc will return a view from the folder VIEW which shpuld have a subfolder named Products and inside it a view named Create.cshtml
        }
        [HttpPost]
        public IActionResult Submit(Product p, [FromServices] ProductsRepository myProductRepository) // model binding in action method injecting the repository via method injection
        {
            //add the product keyed in by the user to the db NOTE NO LINQ code here
            myProductRepository.Add(p);
            TempData["Message"] = "Product added successfully!";
            return View("Index"); // this redirect was necessay because the method name is Submit not Create 
        
        }
        // note: ways of passing data from view => controller
        //1.Parameters  
        //2. FormCollection
        //3. Model Binding

        
        public IActionResult Search (string keyword)
        {
            // note : ways of passing data from controller => view
            //1. Viewbag ; dynamic object,whatever i store inside doesnn't survive redirect
            //2. TemporaryData, survives redirect (only for one request)
            //3. Model
            //4. cookies
            //5. session varaiables

            // search in the db for products matching the keyword
            ViewBag.Message = "no product " + keyword + " found !";
            

            // you control where the user is redirected after the method is executed
            // by default it will look for a view with the same name as the method when you use return View(); ie products/search.cshtml
            // tp redirect to another action method use return View("name of the view");
            return View("Index");
            
        }
       
    }
}
