using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
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

        public IActionResult Index(int page = 1, int pageSize = 9)
        {

            if (TempData["keyword"] != null || TempData["category"] != null)
            {
                //it means that next was pressed in search mode
                string keyword = TempData["keyword"] != null ? TempData["keyword"].ToString() : "";
                int category = TempData["category"] != null ? Convert.ToInt32(TempData["category"]) : -1;


                return Search(keyword, category, page, pageSize);
            }

            //the term models is used for object types that transport data to/from views to/from the controller
            // info 1: List of categories
            var list = myProductsRepository.Get().Skip((page - 1) * pageSize).Take(pageSize);

            var myPreparedSqlofCategories = myCategoriesRepository.GetAllCategories();
            ProductsListViewModel myModel = new ProductsListViewModel();
            myModel.Products = list.ToList();
            myModel.Categories = myPreparedSqlofCategories.ToList();

            //info2 : list of products 
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemsFetched = list.Count();
            ViewBag.PageSize = pageSize;


              return View(myModel); //model: IQueryable<Product>
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
        public IActionResult Create()//this is triggered upon the user clicks the link Create
        {
            var myPreparedSqlofCategories =myCategoriesRepository.GetAllCategories();

            ProductsCreateViewModel myModel= new ProductsCreateViewModel ();
            myModel.Categories = myPreparedSqlofCategories.ToList();//opens connection - gets data - closes connection


            return View(myModel); //mvc will return a view from the folder VIEW which shpuld have a subfolder named Products and inside it a view named Create.cshtml
        }



        //to explain:

        //what happens when there's an error ...redirection!
        //validations
        //revise registration of CategoriesRepository....



        [HttpPost]
        public IActionResult Submit(ProductsCreateViewModel p, [FromServices] IWebHostEnvironment host){ // model binding in action ...method injecting the repository via method injection
            try
            {
                if (p.ImageFile != null)
                {
                    string uniqueFilename = Guid.NewGuid() + System.IO.Path.GetExtension(p.ImageFile.FileName);
                    //by default we save the physical file using the absolute path  \\G:\Enterprise Programming\Enterpise_Programming\EP_Lessons\Presentation\wwwroot\

                    string absolutePath = host.WebRootPath + "//images//"+ uniqueFilename;

                    using (var fileStream = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        p.ImageFile.CopyTo(fileStream);

                    }
                    //relative path is used to render images in the browser
                    string relativePath = @"\images\" + uniqueFilename;
                    p.Product.ImagePath = relativePath;

                }
                 //add the product keyed in by the user to the db NOTE NO LINQ code here
                myProductsRepository.Add(p.Product);
            TempData["success"] = "Product added successfully!";

            //return View("Index"); // this redirect was necessay because the method name is Submit not Create  .. server-side redirection
            return RedirectToAction("Index");
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
        public IActionResult Search (string keyword,int category, int page = 1, int pageSize = 9)
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
            //after 2nd call => Select * From Products Where Name like '%keyword%' or Description Like '%description%'
            //after 3rd call => Select * From Products Where Name like '%keyword%' or Description Like '%description%' order by Name asc
            if (keyword == null) keyword = "";

            var list = myProductsRepository.Get().Where(p => p.Name.Contains(keyword)
                                                 || p.Description.Contains(keyword)
                                                  );

            if (category > 0) list = list.Where(x => x.CategoryFK == category);

            list = list.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalItemsFetched = list.Count();
            ViewBag.PageSize = pageSize;


            //YOU control where the user is redirected after the method completes

            //info 1: list of categories
            var myPreparedSqlOfCategories = myCategoriesRepository.GetAllCategories();
            ProductsListViewModel myModel = new ProductsListViewModel();
            myModel.Products = list.ToList();

            myModel.Categories = myPreparedSqlOfCategories.ToList();


            TempData["keyword"] = keyword;
            TempData["category"] = category;

            return View("Index", myModel);
            //by default => View() it will seek a View called same as the action name i.e. Products\Search.cshtml
            //to redirect the user to a different-named view we use return View("nameOfTheOtherView");


        }

        public IActionResult Details(int id)
        {
            var product = myProductsRepository.Get(id);

            if (product == null)
            {
                TempData["error"] = "Product does not exist";
                return RedirectToAction("Index"); 
            }
            else return View(product);

            //it will redirect the end user to the action (above) called Index



        }

    }
}
