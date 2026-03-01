using Common.Models;
using DataAccess.Context;
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

            // -------------------------------
            // Create Restaurants
            // -------------------------------
            var sushiWave = new Restaurant
            {
                Name = "Sushi Wave",
                Description = "Classic nigiri and creative rolls",
                OwnerEmailAddress = "hana.owner@example.com",
                Address = "45 Marina Street, Sliema",
                Phone = "+356 9876 5432",
                BistrobaseId = "R-001",
                Status = null // Pending
            };

            var marioBistro = new Restaurant
            {
                Name = "Mario's Bistro",
                Description = "Italian food and pizza",
                OwnerEmailAddress = "mario@bistro.com",
                Address = "123 Main Street, Valletta",
                Phone = "+356 1234 5678",
                BistrobaseId = "R-002",
                Status = null
            };

            var oceanGrill = new Restaurant
            {
                Name = "Ocean Grill",
                Description = "Fresh seafood specialists",
                OwnerEmailAddress = "ocean@grill.com",
                Address = "456 Beach Road, St Julian's",
                Phone = "+356 8765 4321",
                BistrobaseId = "R-003",
                Status = null
            };

            // -------------------------------
            // Create MenuItems and assign RestaurantId
            // -------------------------------
            var menuItems = new List<MenuItem>
            {
                // Sushi Wave (Id will be assigned later)
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    BistrobaseId = "M-001",
                    Title = "Salmon Nigiri",
                    Price = 6.50,
                    Currency = "EUR",
                    Status = null,
                    Restaurant = sushiWave
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    BistrobaseId = "M-002",
                    Title = "Dragon Roll",
                    Price = 12.00,
                    Currency = "EUR",
                    Status = null,
                    Restaurant = sushiWave
                },

                // Mario's Bistro
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    BistrobaseId = "M-003",
                    Title = "Tagliatelle al Ragù",
                    Price = 11.50,
                    Currency = "EUR",
                    Status = null,
                    Restaurant = marioBistro
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    BistrobaseId = "M-004",
                    Title = "Margherita Pizza",
                    Price = 9.90,
                    Currency = "EUR",
                    Status = null,
                    Restaurant = marioBistro
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    BistrobaseId = "M-005",
                    Title = "Pepperoni Pizza",
                    Price = 12.00,
                    Currency = "EUR",
                    Status = null,
                    Restaurant = marioBistro
                },

                // Ocean Grill
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    BistrobaseId = "M-006",
                    Title = "Grilled Salmon",
                    Price = 18.90,
                    Currency = "EUR",
                    Status = null,
                    Restaurant = oceanGrill
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    BistrobaseId = "M-007",
                    Title = "Fish and Chips",
                    Price = 15.50,
                    Currency = "EUR",
                    Status = null,
                    Restaurant = oceanGrill
                }
            };

            // -------------------------------
            // Add Restaurants and MenuItems
            // -------------------------------
            context.Restaurants.AddRange(sushiWave, marioBistro, oceanGrill);
            context.MenuItems.AddRange(menuItems);

            // Save changes
            context.SaveChanges();
        }
    }
}