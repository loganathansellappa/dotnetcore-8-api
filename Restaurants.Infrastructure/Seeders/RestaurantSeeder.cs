
﻿using Microsoft.AspNetCore.Identity;
 using Restaurants.Domain.Constants;
 using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext, UserManager<User> _userManager) : IRestaurantSeeder
{
    public async Task Seed()
    {
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
        if (dbContext.Restaurants.Count() < 20)
        {
            var seedUser = new User
            {
                UserName = "testuser@testdevlogan.com",
                Email = "testuser@testdevlogan.com",
                DateOfBirth = DateOnly.Parse("1990-01-01"),
                Nationality = "Indian",
            };
            var result = await _userManager.CreateAsync(seedUser, "abcTest123!");

            var restaurants = GetRandomRestaurants(seedUser.Id, count);
            dbContext.Restaurants.AddRange(restaurants);
            await dbContext.SaveChangesAsync();
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
        var restaurants = new List<Restaurant>();

        for (int i = 1; i <= requiredRecords; i++)
        {
            restaurants.Add(new Restaurant
            {
                Name = $"Restaurant_{i}", // Unique name for each restaurant
                Category = "Fast Food",
                Description = "This is a sample restaurant description for demonstration purposes.",
                ContactEmail = $"contact_{i}@example.com",
                HasDelivery = i % 2 == 0, // Alternate delivery status for variety
                OwnerId = userId,
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = $"Dish_{i}_1",
                        Description = $"Sample Dish {i} (10 pcs.)",
                        Price = 10.30M + i * 0.01M, // Slight variation in price
                    },
                    new Dish
                    {
                        Name = $"Dish_{i}_2",
                        Description = $"Sample Dish {i} (5 pcs.)",
                        Price = 5.30M + i * 0.01M,
                    }
                },
                Address = new Address
                {
                    City = "Sample City",
                    Street = $"Street {i}",
                    PostalCode = $"PC{i:D4}"
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
