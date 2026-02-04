using MediatR;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;

namespace project1.Domains.ItemsDomain.Services
{


    public record DeleteCartItemCommand(int Id) : IRequest<Unit>;
    public class DeleteCartItemHandler : IRequestHandler<DeleteCartItemCommand, Unit>
    {

        private readonly FoodItemDBContext _dbContext;

        public DeleteCartItemHandler(FoodItemDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {

            var deletedCartItem = await _dbContext.CartItems.SingleOrDefaultAsync(food => food.FoodId == request.Id);
            
            if(deletedCartItem == null) { Console.WriteLine("The food you want to delete cannot be found! It seems to be null!"); return Unit.Value; }
            _dbContext.CartItems.Remove(deletedCartItem);
            
            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"The food deleted is {deletedCartItem.FoodName}, id: {deletedCartItem.FoodId}");
            return Unit.Value;
        }
        

        
     }


}