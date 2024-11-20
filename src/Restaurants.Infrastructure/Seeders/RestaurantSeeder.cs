
﻿using Microsoft.AspNetCore.Identity;
 using Microsoft.EntityFrameworkCore;
 using Restaurants.Domain.Constants;
 using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext, UserManager<User> _userManager) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }
        
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
            
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task SeedRandomDataWithHardcodedUser(int count)
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            if (dbContext.Restaurants.Count() < 20)
            {
                var seedUser = await _userManager.FindByEmailAsync("testuser@testdevlogan.com");
                if (seedUser == null)
                {
                    seedUser = new User
                    {
                        UserName = "testuser@testdevlogan.com",
                        Email = "testuser@testdevlogan.com",
                        DateOfBirth = DateOnly.Parse("1990-01-01"),
                        Nationality = "Indian",
                    };
                    var result = await _userManager.CreateAsync(seedUser, "abcTest123!");
                    if (!result.Succeeded)
                    {
                        Console.WriteLine("Failed to create the seed user:");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"- {error.Description}");
                        }
                        return; // Exit if the user creation failed
                    }
                }
                var restaurants = GetRandomRestaurants(seedUser.Id, count);
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
        
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes =
                [
                    new ()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30M,
                    },

                    new ()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30M,
                    },
                ],
                Address = new ()
                {
                    City = "London",
                    Street = "Cork St 5",
                    PostalCode = "WC2N 5DU"
                }
            },
            new ()
            {
                Name = "McDonald",
                Category = "Fast Food",
                Description =
                    "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "London",
                    Street = "Boots 193",
                    PostalCode = "W1F 8SR"
                }
            }
        ];

        return restaurants;
    }

    private IEnumerable<Restaurant> GetRandomRestaurants(string userId, int requiredRecords)
    {
        var restaurantNames = new List<string>
        {
            "Bistro Bella", "The Golden Fork", "Saffron Delight", "Urban Grill", 
            "Ocean Breeze Diner", "The Hungry Chef", "Spice Symphony", "Green Leaf Eatery",
            "Savory Haven", "Rustic Table"
        };

        var categories = new List<string>
        {
            "Italian", "Seafood", "Indian", "American", "Vegan", "BBQ", "Mexican", "Chinese", "Mediterranean", "Fusion"
        };

        var dishesList = new List<(string Name, string Description, decimal BasePrice)>
        {
            ("Margherita Pizza", "Classic pizza with fresh mozzarella and basil.", 12.99M),
            ("Grilled Salmon", "Served with a lemon butter sauce.", 18.99M),
            ("Butter Chicken", "Creamy spiced curry served with naan bread.", 14.99M),
            ("Classic Cheeseburger", "Juicy beef patty with cheddar cheese and pickles.", 10.99M),
            ("Quinoa Salad", "Healthy and fresh with a citrus vinaigrette.", 9.99M),
            ("BBQ Ribs", "Slow-cooked with smoky barbecue sauce.", 16.99M),
            ("Tacos Al Pastor", "Traditional Mexican-style pork tacos.", 8.99M),
            ("Kung Pao Chicken", "Spicy stir-fried chicken with peanuts and chili peppers.", 11.99M),
            ("Falafel Platter", "Served with hummus and pita bread.", 9.99M),
            ("Fusion Sushi Rolls", "Unique sushi with a twist.", 13.99M)
        };

        var cities = new List<string>
        {
            "New York", "San Francisco", "Chicago", "Austin", "Seattle", "Miami", "Portland", "Denver", "Boston", "Las Vegas"
        };

        var restaurants = new List<Restaurant>();

        var random = new Random();

        for (int i = 1; i <= requiredRecords; i++)
        {
            var restaurantName = restaurantNames[random.Next(restaurantNames.Count)];
            var category = categories[random.Next(categories.Count)];
            var city = cities[random.Next(cities.Count)];
            var dish1 = dishesList[random.Next(dishesList.Count)];
            var dish2 = dishesList[random.Next(dishesList.Count)];

            restaurants.Add(new Restaurant
            {
                Name = $"{restaurantName} #{i}", // Ensure uniqueness if there are duplicates
                Category = category,
                Description = $"{restaurantName} is known for its delicious {category} cuisine. Come and enjoy our specialties!",
                ContactEmail = $"{restaurantName.Replace(" ", "").ToLower()}{i}@example.com",
                HasDelivery = random.Next(2) == 0, // Random delivery status
                OwnerId = userId,
                LogoUrl = "",
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = dish1.Name,
                        Description = dish1.Description,
                        Price = dish1.BasePrice + random.Next(1, 5) * 0.5M, // Add small random variation to price
                    },
                    new Dish
                    {
                        Name = dish2.Name,
                        Description = dish2.Description,
                        Price = dish2.BasePrice + random.Next(1, 5) * 0.5M,
                    }
                },
                Address = new Address
                {
                    City = city,
                    Street = $"{random.Next(1, 500)} {restaurantName.Split(' ')[0]} Street", // Unique street name
                    PostalCode = $"PC{random.Next(1000, 9999)}" // Random postal code
                }
            });
        }

        return restaurants;
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
        [
            new(UserRoles.User)
            {
                NormalizedName = UserRoles.User.ToUpperInvariant()
            },
            new(UserRoles.Owner)
            {
                NormalizedName = UserRoles.Owner.ToUpperInvariant()
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpperInvariant()
            }
        ];
        return roles;
    }
}
