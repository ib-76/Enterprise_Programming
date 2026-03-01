using Microsoft.Extensions.Caching.Memory;
using Common.Interfaces;
using Common.Models;

namespace DataAccess.Repositories
{
    public class ItemsMemoryRepository : IItemsRepository
    {
        private readonly IMemoryCache _cache;
        private const string KEY = "importedItems";

        public ItemsMemoryRepository(IMemoryCache cache) => _cache = cache;

        public void Save(List<IitemValidating> items) => _cache.Set(KEY, items);

        public List<IitemValidating>? Get() =>
            _cache.TryGetValue(KEY, out List<IitemValidating> items)
                ? items
                : new List<IitemValidating>();
    }
}