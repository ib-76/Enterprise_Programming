using Common.Interfaces;
using Common.Models;
using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ItemsController : Controller
{
    private readonly ItemsDbRepository _repository;
   

    public ItemsController(ItemsDbRepository repository)
    {
        _repository = repository;
      
    }

    public IActionResult Pending(string simulateUser)
    {
        ViewData["SimulateUser"] = simulateUser;

        string currentUserEmail = simulateUser switch
        {
            "admin" => "admin1@site.com",
            "owner1" => "luca.owner@example.com",
            "owner2" => "hana.owner@example.com",
            _ => "guest@site.com"
        };

        var pendingItems = _repository.GetPending(simulateUser, currentUserEmail);

        return View("Catalogue", pendingItems);
    }

    [HttpPost]
    public IActionResult UpdateStatusBulk(Guid[] selectedIds, string action, string simulateUser)
    {
        bool status = action == "accept";

        _repository.UpdateStatusBulk(selectedIds, status);

        TempData["success"] = $"Updated {selectedIds.Length} item(s) successfully!";
        return RedirectToAction("Pending", new { simulateUser });
    }



}