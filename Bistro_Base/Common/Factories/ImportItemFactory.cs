using Common.Interfaces;
using Common.Models;
using Newtonsoft.Json.Linq;

namespace Services.Factories
{
    public class ImportItemFactory
    {
        public static List<IitemValidating> Create(string json)
        {
            var items = new List<IitemValidating>();
            var array = JArray.Parse(json);
            var restaurants = new Dictionary<string, Restaurant>();

            // First pass: restaurants
            foreach (var obj in array)
            {
                if ((string)obj["type"] == "restaurant")
                {
                    var r = new Restaurant
                    {
                        Id = Guid.NewGuid(),
                        BistrobaseId = (string)obj["id"],
                        Name = (string)obj["name"],
                        Description = (string)obj["description"],
                        OwnerEmailAddress = (string)obj["ownerEmailAddress"],
                        Address = (string)obj["address"],
                        Phone = (string)obj["phone"],
                        Status = null
                    };
                    restaurants[r.BistrobaseId] = r;
                    items.Add(r);
                }
            }

            // Second pass: menu items
            foreach (var token in array)
            {
                var obj = (JObject)token;

                if ((string)obj["type"] == "menuItem")
                {
                    // Trim key in case of extra spaces
                    var restaurantIdProperty = obj.Properties()
                                                  .FirstOrDefault(p => p.Name.Trim() == "restaurantId");

                    var restaurantId = restaurantIdProperty?.Value.ToString();
                    restaurants.TryGetValue(restaurantId, out var r);

                    var m = new MenuItem
                    {
                        Id = Guid.NewGuid(),
                        BistrobaseId = (string)obj["id"],
                        Title = (string)obj["title"],
                        Price = (double?)(obj["price"] ?? 0) ?? 0,
                        Currency = (string)obj["currency"],
                        Status = null,
                        RestaurantId = r?.Id ?? Guid.Empty,
                        Restaurant = r
                    };
                    items.Add(m);
                }
            }

            return items;
        }
    }
}