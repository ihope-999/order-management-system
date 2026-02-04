using System.Globalization;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.ItemsDomain.Services;
using project1.Domains.UserDomain;
using project1.Domains.UserDomain.SessionManager;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class CartModel : PageModel
    {


        public class CartUpdate
        {
            public int FoodId { get; set; }
            public int FoodQuantity { get; set; }
        }

        public bool isLogged { get; set; } = false;


        public Cart Cart { get; set; } = new();
        public FoodItemDBContext _dbContext;
        public IMediator _mediator;
        public List<CartItem> CartItems { get; set; } = new();

        public UserManager<User> _userManager;
        public IRegisterUserHandler _registerUserHandler;

        public ISessionManager _sessionManager;
        //public User UserFound  { get; set; }
        public string SessionId     { get; set; }
        public string UserId { get; set; }
        [BindProperty]
        public int FoodQuantity { get; set; }
        public CartModel(FoodItemDBContext dbContext, IMediator mediator, ISessionManager sessionManager, IRegisterUserHandler registerUserHandler ,UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _sessionManager = sessionManager;
            _userManager = userManager;
            _registerUserHandler = registerUserHandler;

        }


        public IActionResult OnPostGoToCheckOut()
        {
            return RedirectToPage("/Checkout"); 
        }
        public async Task OnGetAsync()
        {
            isLogged = await _registerUserHandler.CheckLoginStatus(User);
            var myUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            UserId = myUser.Id;
            Cart = await _dbContext.Carts.Include(u => u.Items).Include(u => u.Status).FirstOrDefaultAsync(cart => (cart.CartId == SessionId || cart.CartId == UserId) && (!cart.Status.isOrdered));
                if (Cart == null)
                {
                    Cart = new Cart();
                    Console.WriteLine("------------------------NEW Cart Created--------------------------------");
                    if (isLogged)
                    {
                        Cart.CartId = UserId;
                    }
                    else
                    {
                        SessionId = _sessionManager.GetOrCreateSessionId();
                        Cart.CartId = SessionId;
                    }
                }
                //CartItems = await _dbContext.CartItems.Where(cart => cart.CartId == UserId || cart.CartId == SessionId).ToListAsync();
                Console.WriteLine($"Your unique identifier is IF LOGGED---: {UserId} ----- or if NOT LOGGED:-----{SessionId}");
            }
        


        public async Task<IActionResult> OnPostDeleteFromTheCartAsync(int id)
        {
            Cart = await _dbContext.Carts.Include(u => u.Items).FirstOrDefaultAsync(cart => cart.CartId == SessionId || cart.CartId == UserId);

            var command = new DeleteCartItemCommand(id);
            await _mediator.Send(command);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostIncreaseFoodCountByOneAsync(int id)
        {
            FoodQuantity++;
            var increasedCartItem = await _dbContext.CartItems.FindAsync(id);
            Console.WriteLine($"The food quantity of {increasedCartItem.FoodName} is {FoodQuantity}");
            increasedCartItem.CountOfItem = FoodQuantity;
            await _dbContext.SaveChangesAsync();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDecreaseFoodCountByOne(int id)
        {
            FoodQuantity--;
            var increasedCartItem = await _dbContext.CartItems.FindAsync(id);
            Console.WriteLine($"The food quantity of {increasedCartItem.FoodName} is {FoodQuantity}");
            increasedCartItem.CountOfItem = FoodQuantity;
            await _dbContext.SaveChangesAsync();
            return RedirectToPage();
        }





        public async Task<IActionResult> OnPostIncreaseFoodCountAsync([FromBody] CartUpdate request)
        {
            if (request == null)
                return new JsonResult(new { success = false, message = "Invalid data" });

            var increasedCartItem = await _dbContext.CartItems.FindAsync(request.FoodId);
            if (increasedCartItem != null)
            {
                increasedCartItem.CountOfItem = request.FoodQuantity;
                await _dbContext.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false, message = "Item not found" });
        }

    }
}
