using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
namespace project1.Domains.ItemsDomain.Interface
{
    public interface IFoodItemHandler
    {

        public Task AddFoodItemToTheDatabase(FoodItem foodItem);
        public Task RemoveFoodItemFromTheDatabase(FoodItem foodItem);

        public Task UpdateFoodItemFromTheDatabase(string foodName, FoodItem updatedFood);
    }
}
