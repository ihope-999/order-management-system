using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
namespace project1.Domains.ItemsDomain.Services.FoodHandlers

{

    public record AddFoodCommand( FoodItem FoodItem, string email) : IRequest<Unit>;
    public class AddFoodHandler : IRequestHandler<AddFoodCommand, Unit>
    {
        private readonly FoodItemDBContext _dbContext;

        public AddFoodHandler(FoodItemDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Unit> Handle(AddFoodCommand request, CancellationToken cancellationToken)
        {
            request.FoodItem.Seller = request.email;
            await _dbContext.FoodItems.AddAsync(request.FoodItem);
            
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }

    }
}
