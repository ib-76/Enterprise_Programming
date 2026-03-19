using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;

public class ItemsController : Controller
{
    private readonly IItemsRepository _dbRepo;

    // Simulated logged-in user (hardcoded)
    private readonly string _currentUser;


    public ItemsController([FromKeyedServices("db")] IItemsRepository dbRepository)
    {
        //_currentUser = "admin1@site.com"; // Change to "hana.owner@example.com" to simulate  "admin1@site.com"  luca.owner@example.com
        _dbRepo = dbRepository;
    }

    // INDEX: default approved restaurants
    public IActionResult Index()
    {
        var allItems = _dbRepo.Get();
        var currentUser = User.Identity.Name;

        List<IitemValidating> approvedRestaurants;

        if (currentUser == "admin1@site.com")
        {
            // Admin sees all approved restaurants
            approvedRestaurants = allItems
                .OfType<Restaurant>()
                .Where(r => r.Status == true)
                .Cast<IitemValidating>()
                .ToList();
        }
        else
        {
            // Non-admin users see only their own approved restaurants
            approvedRestaurants = allItems
                .OfType<Restaurant>()
                .Where(r => r.Status == true && r.OwnerEmailAddress == currentUser)
                .Cast<IitemValidating>()
                .ToList();
        }

        ViewData["viewType"] = "approved";
        return View("catalogue", approvedRestaurants);
    }





    // DETAILS: menu items of a restaurant
    public IActionResult Details(Guid restaurantId)
    {
        var items = _dbRepo.Get();

        var menuItems = items
            .OfType<MenuItem>()
            .Where(m => m.Restaurant.Id == restaurantId && m.Status == true)
            .Cast<IitemValidating>()
            .ToList();

        //ViewData["simulateUser"] = _currentUser;
        return View("catalogue", menuItems);
    }

    // VERIFICATION: shows pending depending on user
    public IActionResult Verification()
    {
        var currentUser = User.Identity.Name;

        List<IitemValidating> pendingToShow;

        if (currentUser == "admin1@site.com")   // mario.owner @example.com
        {
            pendingToShow = _dbRepo.Get()
                .OfType<Restaurant>()
                .Where(r => r.Status == null)
                .Cast<IitemValidating>()
                .ToList();
        }
        else
        {
            pendingToShow = _dbRepo.Get()
                .OfType<MenuItem>()
                .Where(m => m.Status == null
                           && m.Restaurant.OwnerEmailAddress == currentUser 
                           && m.Restaurant.Status == true
                         )
                .Cast<IitemValidating>()
                .ToList();
        }

        ViewData["viewType"] = "pending";

        return View("catalogue", pendingToShow);
    }



        // Approve selected restaurants or menu items
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter))]
    public IActionResult ApproveStatus(Guid[] selectedIds)
    {
        var items = _dbRepo.Get();

        foreach (var item in items)
        {
            if (item is Restaurant r && selectedIds.Contains(r.Id))
                r.Status = true;

            if (item is MenuItem m && selectedIds.Contains(m.Id))
                m.Status = true;
        }

        _dbRepo.Save(items);

        return RedirectToAction("Index");
    }
}