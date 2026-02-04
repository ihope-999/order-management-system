using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.PaymentDomain.PaymentItem;
using project1.Domains.UserDomain.SessionManager;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class BuyTheProductsModel : PageModel
    {

        public User myUser { get; set; }
        public FoodItemDBContext _dbContext;
        //public List<CartItem> cartItems{ get; set; }
        public Cart Cart { get; set; }
        public ISessionManager _sessionManager {  get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        [BindProperty]
        public int paymentInfoId { get; set; }

        public bool showSavedPaymentInfo { get; set; } = false;

        [BindProperty]
        public PaymentInfo PaymentInfo { get; set; } 
        public BuyTheProductsModel(FoodItemDBContext dbContext, ISessionManager sessionManager)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
        }
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                SessionId = _sessionManager.GetOrCreateSessionId();
            }
            //cartItems = await _dbContext.CartItems.Where(cartItem => cartItem.CartId == UserId || cartItem.CartId == SessionId).ToListAsync();
            Cart = await _dbContext.Carts.Include(u => u.Items).Include(u=>u.Status).FirstOrDefaultAsync(cart => cart.CartId == UserId || cart.CartId == SessionId);

        }

        public async Task<IActionResult> OnPostBuyTheProductsAsync()
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart = await _dbContext.Carts.Include(u => u.Items).Include(u => u.Status).FirstOrDefaultAsync(cart =>( cart.CartId == UserId || cart.CartId == SessionId) && !cart.Status.isOrdered);

            if (User.Identity.IsAuthenticated)
            {
                myUser = await _dbContext.Users.Include(user=> user.PaymentInfo).FirstOrDefaultAsync(user => user.Id == UserId);
                PaymentInfo.UserId = myUser.Id; 
                myUser.PaymentInfo.Add(PaymentInfo);
                   
            }
            else
            {
                Console.WriteLine("You need to login first to proceed!");
                return RedirectToPage("/Register");
            }

            Cart.Status.IsPaid = true;
            Cart.Status.isOrdered = true;
            Cart.paymentInfoId = paymentInfoId;
            
            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"-----------You ORDERED the items! YOUR STATUS OF THE CART IS {Cart.Status.isOrdered} and status id is {Cart.Status.CartId == Cart.CartId}--------------------");
            Console.WriteLine("-----------You PAID the items!--------------------");
            Console.WriteLine("-----------You are currently waiting for the shipper to ACCEPT your ORDER--------------------");

            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostBuyTheProductsWithSavedInformationAsync()
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart = await _dbContext.Carts.Include(u => u.Items).Include(u => u.Status).FirstOrDefaultAsync(cart => (cart.CartId == UserId || cart.CartId == SessionId) && !cart.Status.isOrdered);

            if (User.Identity.IsAuthenticated)
            {
                myUser = await _dbContext.Users.Include(user => user.PaymentInfo).FirstOrDefaultAsync(user => user.UserId == UserId);
            }
            else
            {
                Console.WriteLine("You need to login first to proceed!");
                return RedirectToPage("/Register");
            }

            Cart.Status.IsPaid = true;
            Cart.Status.isOrdered = true;
            Cart.paymentInfoId = paymentInfoId;

            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"-----------You ORDERED the items! YOUR STATUS OF THE CART IS {Cart.Status.isOrdered} and status id is {Cart.Status.CartId == Cart.CartId}--------------------");

            Console.WriteLine($"-----------You PAID the items! with SAVED PAYMENT INFORMATION {paymentInfoId} IS THE SAVED PAYMENT INFO--------------------");
            Console.WriteLine("-----------You are currently waiting for the shipper to ACCEPT your ORDER--------------------");

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostShowPaymentInformationsAsync()
        {

            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart = await _dbContext.Carts.Include(u => u.Items).Include(u => u.Status).FirstOrDefaultAsync(cart => (cart.CartId == UserId || cart.CartId == SessionId) && !cart.Status.isOrdered);

            if (User.Identity.IsAuthenticated)
            {
                myUser = await _dbContext.Users.Include(user => user.PaymentInfo).FirstOrDefaultAsync(user => user.UserId == UserId);
                PaymentInfo.UserId = myUser.Id;
                myUser.PaymentInfo.Add(PaymentInfo);

            }
            else
            {
                Console.WriteLine("You need to login first to proceed!");
                return RedirectToPage("/Register");
            }

            Console.WriteLine("--------------------You WANT SAVED PAYMENT INFO-----------------------");
            myUser = await _dbContext.Users.Include(user => user.PaymentInfo).FirstOrDefaultAsync(user => user.UserId == UserId);

            showSavedPaymentInfo = true;
            return Page();
        }

    }
}
