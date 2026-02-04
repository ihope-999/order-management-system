using MediatR;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;

namespace project1.Domains.ItemsDomain.Services.FoodHandlers
{


    public record UpdateCommand(FoodItem updatedFood) : IRequest<Unit>;
    public class UpdateHandler : IRequestHandler<UpdateCommand, Unit>
    {
        public FoodItemDBContext _dbContext { get; set; }
        public UpdateHandler(FoodItemDBContext dbContext)
        {

            _dbContext = dbContext;

        }
        public async Task<Unit> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var existingFood = await _dbContext.FoodItems.FindAsync(request.updatedFood.FoodId);
            if (existingFood == null) return Unit.Value;
            existingFood.FoodName = request.updatedFood.FoodName;
            existingFood.FoodDescription = request.updatedFood.FoodDescription;
            existingFood.FoodPrice = request.updatedFood.FoodPrice;
            existingFood.FoodDeliverer = request.updatedFood.FoodDeliverer;
            existingFood.FoodDeliveryAddress = request.updatedFood.FoodDeliveryAddress;
            existingFood.FoodTimeToMake = request.updatedFood.FoodTimeToMake;
            existingFood.FoodTimeToMakeTheDelivery = request.updatedFood.FoodTimeToMakeTheDelivery;
            Console.WriteLine($"Food update is completed, updated food is {request.updatedFood.FoodId}");
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
