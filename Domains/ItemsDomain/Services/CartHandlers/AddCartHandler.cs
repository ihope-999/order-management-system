using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
namespace project1.Domains.ItemsDomain.Services

{

    public record AddCartCommand(CartItem addToCartItem,string SessionId, string UserId) : IRequest<Unit>;
    public class AddToTheCartHandler : IRequestHandler<AddCartCommand, Unit>
    {
        private readonly FoodItemDBContext _dbContext;

        public AddToTheCartHandler(FoodItemDBContext dbContext)
        {
            _dbContext = dbContext;

        }


        public async Task<Unit> Handle(AddCartCommand request, CancellationToken cancellationToken)
  

        {
            Cart Cart = await _dbContext.Carts.Include(u=> u.Items).Include(u=>u.Status).FirstOrDefaultAsync(cart => (cart.CartId == request.SessionId || cart.CartId == request.UserId) && cart.Status.isOrdered == false);

            Console.WriteLine("-------------------------------CART HAS BEEN SECURED IT CAN BE NULL OR NOT!----------------");

            if (Cart == null) {

                Cart = new Cart();
                await _dbContext.Carts.AddAsync(Cart);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"------------------------The Id of the Cart is {Cart.Id} and status id {Cart.Status.Id}-------------------------------------------");



            }
            if (request.UserId != null)
            {
                Console.WriteLine("You are logged in!");
                Cart.CartId = request.UserId;
                Cart.Status.CartId = request.UserId;
                
                request.addToCartItem.CartId = request.UserId;


            }
            else
            {
                Cart.CartId = request.SessionId;
                Cart.Status.CartId = request.SessionId;

                Console.WriteLine("You are not logged in!");

            }
            Console.WriteLine("--------------------------------You added the STATUS ID and ID of the CART-------------------------------");
            var checkedFood = Cart.Items.FirstOrDefault(cartItem => cartItem._FoodIdQ == request.addToCartItem._FoodIdQ && (cartItem.CartId == request.UserId || cartItem.CartId == request.SessionId));


            //var checkedFood = await _dbContext.CartItems.FirstOrDefaultAsync(cartItem => cartItem._FoodIdQ == request.addToCartItem._FoodIdQ && (cartItem.CartId == request.UserId ||  cartItem.CartId == request.SessionId));
            if (checkedFood == null)
            {
                Cart.Items.Add(request.addToCartItem);
                Console.WriteLine("null food");
                //await _dbContext.CartItems.AddAsync(request.addToCartItem);
            }
            else
            {
                checkedFood.CountOfItem++;
                Console.WriteLine("not null food");


                //checkedFood.CountOfItem++;
            }
            Console.WriteLine($"----------------The KEY ID of CART is {Cart.Id}------------------------");
            await _dbContext.SaveChangesAsync();

            


                return Unit.Value;
       



        }



    }
}
