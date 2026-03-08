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

        return View("catalogue", pending);
    }

    [HttpPost]
    public IActionResult UpdateStatusBulk(Guid[] selectedIds, string action, string simulateUser)
    {
        bool status = action == "accept";
        var items = _dbRepo.Get();

        foreach (var item in items)
        {
            switch (item)
            {
                case Restaurant r when selectedIds.Contains(r.Id):
                    r.Status = status;
                    break;

                case MenuItem m when selectedIds.Contains(m.Id):
                    m.Status = status;
                    break;
            }
        }

        _dbRepo.Save(items);
        return RedirectToAction("Pending", new { simulateUser });
    }
}