using Common.Interfaces;
using Common.Models;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

public class ItemsDbRepository : IItemsRepository
{
    private readonly CatalogueDbContext _context;

    public ItemsDbRepository(CatalogueDbContext context)
    {
        _context = context;
    }

    public void Save(List<IitemValidating> items)
    {
        var restaurants = items.OfType<Restaurant>().ToList();
        var menuItems = items.OfType<MenuItem>().ToList();

        // -------- RESTAURANTS --------
        foreach (var r in restaurants)
        {
            var existing = _context.Restaurants
                .FirstOrDefault(x => x.BistrobaseId == r.BistrobaseId);

            if (existing == null)
            {
                r.Id = Guid.NewGuid();
                r.Status = null;
                _context.Restaurants.Add(r);
            }
            else
            {
                // Update all fields including Status
                existing.Name = r.Name;
                existing.Description = r.Description;
                existing.OwnerEmailAddress = r.OwnerEmailAddress;
                existing.Address = r.Address;
                existing.Phone = r.Phone;
                existing.ImagePath = r.ImagePath;
                existing.Status = r.Status;  // <-- update approval status
            }
        }

        _context.SaveChanges(); // commit restaurants first

        // -------- MENU ITEMS --------
        foreach (var m in menuItems)
        {
            // Map to correct DB restaurant
            var dbRestaurant = _context.Restaurants
                                       .FirstOrDefault(r => r.BistrobaseId == m.Restaurant.BistrobaseId);
            if (dbRestaurant == null) continue; // skip orphan menu items

            m.RestaurantId = dbRestaurant.Id;

            var existing = _context.MenuItems
                .FirstOrDefault(x => x.BistrobaseId == m.BistrobaseId);

            if (existing == null)
            {
                m.Id = Guid.NewGuid();
                m.Status = null;
                _context.MenuItems.Add(m);
            }
            else
            {
                existing.Title = m.Title;
                existing.Price = m.Price;
                existing.Currency = m.Currency;
                existing.ImagePath = m.ImagePath;
                existing.RestaurantId = m.RestaurantId;
                existing.Status = m.Status; // <-- update approval status
            }
        }

        _context.SaveChanges(); // commit menu items
    }

    public List<IitemValidating> Get()
    {
        var restaurants = _context.Restaurants
            .Include(r => r.MenuItems)
            .Cast<IitemValidating>()
            .ToList();

        var menuItems = _context.MenuItems
            .Include(m => m.Restaurant)
            .Cast<IitemValidating>()
            .ToList();

        return restaurants.Concat(menuItems).ToList();
    }
}