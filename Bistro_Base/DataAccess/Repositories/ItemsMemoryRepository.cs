using Microsoft.Extensions.Caching.Memory;
using Common.Interfaces;
using Common.Models;

namespace DataAccess.Repositories
{
    public class ItemsMemoryRepository : IItemsRepository
    {
        private readonly IMemoryCache _cache;
        private const string KEY = "importedItems"; // single key to hold all items

        // Inject IMemoryCache via DI
        public ItemsMemoryRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Save list of items to cache
        public void Save(List<IitemValidating> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            // You can optionally set cache expiration here
            _cache.Set(KEY, items);
        }


        // Retrieve list of items from cache
        public List<IitemValidating> Get()
        {
            // Try to get items from the cache
            if (_cache.TryGetValue(KEY, out List<IitemValidating> items))
            {
                return items; // return cached items
            }

            // Return empty list if nothing is cached
            return new List<IitemValidating>();
        }
    }
}