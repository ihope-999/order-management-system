using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class AwaitingShipmentsModel : PageModel
    {

        private readonly FoodItemDBContext _dbContext;
        private readonly ILogger<AwaitingShipmentsModel> _logger;
        private readonly UserManager<User> _userManager;
        public ICollection<CartItem> cartItemsWaiting = new List<CartItem>();
        public AwaitingShipmentsModel(ILogger<AwaitingShipmentsModel> logger,FoodItemDBContext dBContext, UserManager<User> userManager)
        {
            _logger = logger;
            _dbContext = dBContext;
            _userManager = userManager;


        }


        public async Task OnGetAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            cartItemsWaiting = await _dbContext.CartItems.Include(u=> u.Cart).Include(u=> u.Cart.Status).Where(cartItem => (cartItem.Seller == user.Email && cartItem.Cart.Status.isOrdered)).ToListAsync();
            
            if(cartItemsWaiting != null)
            {
                _logger.LogWarning("The items waiting for shipment ARE NOT NULL ");
            }


        }
        public async Task OnPostItemGivenToTheCargoAsync(int FoodId, string cartId)
        {


        }
    }
}
