using MediatR;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;

namespace project1.Domains.ItemsDomain.Services.FoodHandlers
{


    public record DeleteCommand(int Id) : IRequest<Unit>;
    public class DeleteHandler : IRequestHandler<DeleteCommand, Unit>
    {

        private readonly FoodItemDBContext _dbContext;

        public DeleteHandler(FoodItemDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {

            var deletedFood = await _dbContext.FoodItems.SingleOrDefaultAsync(food => food.FoodId == request.Id);

            if (deletedFood == null) { Console.WriteLine("The food you want to delete cannot be found! It seems to be null!"); return Unit.Value; }
            _dbContext.FoodItems.Remove(deletedFood);

            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"The food deleted is {deletedFood.FoodName}, id: {deletedFood.FoodId}");
            return Unit.Value;
        }



    }


}