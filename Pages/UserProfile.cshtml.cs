using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class UserProfileModel : PageModel
    {

        public FoodItemDBContext _dbContext { get; set; }
        public User user { get; set; }

        private readonly UserManager<User> _userManager;
        public string UserId { get; set; }
        public ICollection<string> roles { get; set; }

        public UserProfileModel(FoodItemDBContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("You are not logged in!");
                return RedirectToPage("Index");
                
            }
            else
            {
                Console.WriteLine("---------------YOU ARE LOGGED IN AND CAN ACCESS TO USERPROFILE---------------");
            }
            user = await _userManager.Users.Include(u=> u.PaymentInfo).FirstOrDefaultAsync(user => user.Email == User.Identity.Name);
            roles = await _userManager.GetRolesAsync(user);

            if (user == null) Console.WriteLine("USER IS NULL FOR THE PAGE");
          

            return Page();
        }
    }
}
