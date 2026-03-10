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

        foreach (var r in restaurants)
        {
            r.ImagePath ??= "wwwroot/Images/restaurants/default.jpg";

            if (_context.Restaurants.Any(x => x.Id == r.Id))
                _context.Restaurants.Update(r);
            else
                _context.Restaurants.Add(r);
        }

        foreach (var m in menuItems)
        {
            m.ImagePath ??= "wwwroot/Images/menuitems/default.jpg";

            if (_context.MenuItems.Any(x => x.Id == m.Id))
                _context.MenuItems.Update(m);
            else
                _context.MenuItems.Add(m);
        }

        _context.SaveChanges();
    }


    public List<IitemValidating> Get()
    {
        var restaurants = _context.Restaurants.Include(r => r.MenuItems).Cast<IitemValidating>().ToList();
        var menuItems = _context.MenuItems.Include(m => m.Restaurant).Cast<IitemValidating>().ToList();
        return restaurants.Concat(menuItems).ToList();
    }
}