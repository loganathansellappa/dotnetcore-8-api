namespace Restaurants.Infrastructure.Seeders
{
    public interface IRestaurantSeeder
    {
        Task Seed();
        Task SeedRandomDataWithHardcodedUser(int count);
    }
}