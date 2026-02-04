using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.PaymentDomain.PaymentItem;

namespace project1.Pages
{
    public class ShippingDetailsModel : PageModel
    {
        public FoodItemDBContext _dbContext;
        public PaymentInfo PaymentInfo { get; set; }
        public List<CartItem> CartItems { get; set; } = new();
        public ShippingDetailsModel(FoodItemDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGetAsync()
        {
            CartItems = await _dbContext.CartItems.ToListAsync();
        }


    }
}
