using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

public class ItemsController : Controller
{
    private readonly IItemsRepository _dbRepo;

    public ItemsController([FromKeyedServices("db")] IItemsRepository dbRepository)
    {
        _dbRepo = dbRepository;
    }

    // METHOD 1: DEFAULT CATALOGUE
    public IActionResult Index(string viewType = "approved")
    {
        var allItems = _dbRepo.Get();

        // Only restaurants
        var restaurants = allItems.OfType<Restaurant>().ToList(); // CAST to Restaurant

        // Filter by viewType
        var restaurantsToShow = viewType switch
        {
            "pending" => restaurants.Where(r => r.Status == null).ToList(),
            _ => restaurants.Where(r => r.Status == true).ToList()
        };

        ViewData["viewType"] = viewType;
        ViewData["simulateUser"] = "guest"; // or your simulated user

        return View("catalogue", restaurantsToShow.Cast<IitemValidating>());
    }


    // METHOD 2: DETAILS -> SHOW MENU ITEMS OF RESTAURANT
    public IActionResult Details(Guid restaurantId, string simulateUser)
    {
        var items = _dbRepo.Get();

        var menuItems = items
            .OfType<MenuItem>()
            .Where(m => m.Restaurant.Id == restaurantId && m.Status == true)
            .Cast<IitemValidating>()
            .ToList();

        ViewData["simulateUser"] = simulateUser;

        return View("catalogue", menuItems);
    }


    // METHOD 3: PENDING (your existing method)
    public IActionResult Pending(string simulateUser)
    {
        var allItems = _dbRepo.Get();

        string currentUserEmail = simulateUser switch
        {
            "admin" => "admin1@site.com",
            "owner1" => "luca.owner@example.com",
            "owner2" => "hana.owner@example.com",
            _ => "guest@site.com"
        };

        List<IitemValidating> pending = simulateUser switch
        {
            "admin" => allItems.OfType<Restaurant>()
                               .Where(r => r.Status == null)
                               .Cast<IitemValidating>()
                               .ToList(),

            "owner1" or "owner2" => allItems.OfType<MenuItem>()
                                           .Where(m => m.Status == null &&
                                                       m.Restaurant.OwnerEmailAddress == currentUserEmail)
                                           .Cast<IitemValidating>()
                                           .ToList(),

            _ => new List<IitemValidating>()
        };

        ViewData["simulateUser"] = simulateUser;

        return View("catalogue", pending);
    }



    [HttpPost]
    public IActionResult UpdateStatusBulk(Guid[] selectedIds, string simulateUser)
    {
        if (selectedIds == null || selectedIds.Length == 0)
            return RedirectToAction("Index", new { simulateUser });

        // Get all items from repo
        var allItems = _dbRepo.Get();

        foreach (var item in allItems)
        {
            if (item is Restaurant r && selectedIds.Contains(r.Id))
            {
                r.Status = true; // Approve
            }
        }

        _dbRepo.Save(allItems);

        // Redirect to the same catalogue view
        return RedirectToAction("Index", new { simulateUser });
    }

}

