using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.Interface;
namespace project1.Domains.ItemsDomain.Services.FoodHandlers
{
    public class FoodItemHandler : IFoodItemHandler
    {
        public FoodItemDBContext _foodItems;
        public FoodItemHandler(FoodItemDBContext foodItems)
        {
            _foodItems = foodItems;
        }
        public async Task AddFoodItemToTheDatabase(FoodItem foodItem)
        {
            _foodItems.Add(foodItem);
            Console.WriteLine($"The Food {foodItem.FoodName} is added to the database");
            await _foodItems.SaveChangesAsync();
        }

        public async Task RemoveFoodItemFromTheDatabase(FoodItem foodItem)
        {
            if (foodItem != null)
            {
                _foodItems.Remove(foodItem);
                Console.WriteLine($"The Food {foodItem.FoodName} is remvoed from the database");
                await _foodItems.SaveChangesAsync();
            }


        }

        public async Task UpdateFoodItemFromTheDatabase(string foodName, FoodItem updatedFood)
        {

            var existingFood = _foodItems.FoodItems.FirstOrDefault(foodItem => foodItem.FoodName == foodName);
            if (existingFood != null)
            {
                updatedFood.FoodName = existingFood.FoodName;
                _foodItems.FoodItems.Update(updatedFood);
                await _foodItems.SaveChangesAsync();

            }
            Console.WriteLine($"The Food {updatedFood.FoodName} is added to the database, the old food was {existingFood.FoodName} ");


        }


    }
}
