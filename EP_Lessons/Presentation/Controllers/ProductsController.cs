using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
