using Common.Interfaces;
using Common.Models;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ItemsDbRepository : IItemsRepository
    {
        private readonly CatalogueDbContext _context;

        public ItemsDbRepository(CatalogueDbContext context) => _context = context;

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

        public void Save(List<IitemValidating> items)
        {
            var restaurants = items.OfType<Restaurant>().ToList();
            var menuItems = items.OfType<MenuItem>().ToList();

            foreach (var r in restaurants)
                if (string.IsNullOrEmpty(r.ImagePath))
                    r.ImagePath = "wwwroot/Images/restaurants/default.jpg";

            foreach (var m in menuItems)
                if (string.IsNullOrEmpty(m.ImagePath))
                    m.ImagePath = "wwwroot/Images/menuitems/default.jpg";

            _context.Restaurants.AddRange(restaurants);
            _context.MenuItems.AddRange(menuItems);

            _context.SaveChanges();
        }

        public void Update(List<IitemValidating> items)
        {
            foreach (var item in items)
            {
                if (item is Restaurant r)
                {
                    _context.Restaurants.Update(r);
                }
                else if (item is MenuItem m)
                {
                    _context.MenuItems.Update(m);
                }
            }
            _context.SaveChanges();
        }


        public void UpdateStatusBulk(Guid[] selectedIds, bool status)
        {
            var restaurants = _context.Restaurants.Where(r => selectedIds.Contains(r.Id)).ToList();
            var menuItems = _context.MenuItems.Where(m => selectedIds.Contains(m.Id)).ToList();

            foreach (var r in restaurants)
                r.Status = status;

            foreach (var m in menuItems)
                m.Status = status;

            _context.SaveChanges();
        }

            public List<IitemValidating> GetPending(string simulateUser, string currentUserEmail)
        {
            if (simulateUser == "admin")
            {
                return _context.Restaurants
                    .Where(r => r.Status == null)
                    .Cast<IitemValidating>()
                    .ToList();
            }

            return _context.MenuItems
                .Include(m => m.Restaurant)
                .Where(m => m.Status == null && m.Restaurant.OwnerEmailAddress == currentUserEmail)
                .Cast<IitemValidating>()
                .ToList();
        }
    }
  

}