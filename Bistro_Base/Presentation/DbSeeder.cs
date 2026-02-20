using Common.Models;
using DataAccess.Context;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation
{
    public static class DbSeeder
    {
        public static void Seed(CatalogueDbContext context)
        {
            // Prevent duplicate seeding
            if (context.Restaurants.Any())
                return;

            // Restaurants (based on JSON)
            var sushiWave = new Restaurant
            {
                Name = "Sushi Wave",
                Description = "Classic nigiri and creative rolls",
                OwnerEmailAddress = "hana.owner@example.com",
                Address = "45 Marina Street, Sliema",
                Phone = "+356 9876 5432",
                BistrobaseId = "R-001",
            };

            var marioBistro = new Restaurant
            {
                Name = "Mario's Bistro",
                Description = "Italian food and pizza",
                OwnerEmailAddress = "mario@bistro.com",
                Address = "123 Main Street, Valletta",
                Phone = "+356 1234 5678",
                BistrobaseId = "R-002",
            };

            var oceanGrill = new Restaurant
            {
                Name = "Ocean Grill",
                Description = "Fresh seafood specialists",
                OwnerEmailAddress = "ocean@grill.com",
                Address = "456 Beach Road, St Julian's",
                Phone = "+356 8765 4321",
                BistrobaseId = "R-003",

            };

            // MenuItems using navigation property

            sushiWave.MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Salmon Nigiri",
                    Price = 6.50,
                    Currency = "EUR",
                    BistrobaseId = "M-001",
                   

                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Dragon Roll",
                    Price = 12.00,
                    Currency = "EUR",
                    BistrobaseId = "M-002",
                     
                }
            };

            marioBistro.MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Tagliatelle al Ragù",
                    Price = 11.50,
                    Currency = "EUR",
                    BistrobaseId = "M-003",
                     
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Margherita Pizza",
                    Price = 9.90,
                    Currency = "EUR",
                    BistrobaseId = "M-004",
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Pepperoni Pizza",
                    Price = 12.00,
                    Currency = "EUR",
                    BistrobaseId = "M-005",

                }
            };

            oceanGrill.MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Grilled Salmon",
                    Price = 18.90,
                    Currency = "EUR",
                    BistrobaseId = "M-006",

                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Fish and Chips",
                    Price = 15.50,
                    Currency = "EUR",
                    BistrobaseId = "M-007",

                }
            };

            // Add everything
            context.Restaurants.AddRange(sushiWave, marioBistro, oceanGrill);

            // Save
            context.SaveChanges();
        }
    }
}
