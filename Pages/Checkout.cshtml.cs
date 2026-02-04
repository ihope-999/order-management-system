using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.UserDomain.SessionManager;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class CheckoutModel : PageModel
    {

        public FoodItemDBContext _dbContext;

        public Cart Cart { get; set; } = new();
        //public List<CartItem> CartItems { get; set; } = new();
        public double TotalPrice { get; set; }
        public string UserId {  get; set; }
        public string SessionId { get; set; }
        private readonly ISessionManager _sessionManager;
        private readonly UserManager<User> _userManager;
        public CheckoutModel(FoodItemDBContext dbContext, ISessionManager sessionManager, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
            _userManager = userManager;

        }


      
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                UserId = user.Id;
                if(UserId != null)
                {
                    Console.WriteLine("-------------USER FOUND----------------");
                }
                else
                {
                    Console.WriteLine("-------------USER NOT FOUND----------------");

                }
            }
            else
            {
                SessionId = _sessionManager.GetOrCreateSessionId();
            }

            Console.WriteLine("OnGet for Checkout");

            Cart = await _dbContext.Carts.Include(u => u.Items).Include(i => i.Status).FirstOrDefaultAsync(cart =>( cart.CartId == SessionId || cart.CartId == UserId)&& !Cart.Status.isOrdered);
            //CartItems = await _dbContext.CartItems.Where(cartItem => cartItem.CartId == SessionId || cartItem.CartId == UserId).ToListAsync();
            foreach (var cartItem in Cart.Items)
            {
                TotalPrice += cartItem.FoodPrice * cartItem.CountOfItem;
                
            }
            Console.WriteLine("Total price " + TotalPrice);

        }

        public IActionResult OnPostBuyTheProducts()
        {
            return RedirectToPage("/BuyTheProducts");
        }

    }
}
