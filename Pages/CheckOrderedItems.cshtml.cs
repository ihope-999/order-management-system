using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;

namespace project1.Pages
{
    public class CheckOrderedItemsModel : PageModel
    {

        public List<Cart> Carts { get; set; } = new();
        public FoodItemDBContext _dbContext { get; set; }

       
        public string UserId { get; set; }
        
        public CheckOrderedItemsModel(FoodItemDBContext dbContext) 
        { 
            _dbContext = dbContext;
        
        }
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated) 
            { 
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            Carts = await _dbContext.Carts.Include(u=> u.Items).Include(l => l.Status).Where(cart => (cart.CartId == UserId) && cart.Status.isOrdered).ToListAsync();
            if (Carts == null) 
            {
                Console.WriteLine("--------------------You do not have ordered any items------------------------");
            }

        }
    }
}
