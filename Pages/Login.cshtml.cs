using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class LoginModel : PageModel
    {

        public FoodItemDBContext _dbContext;
        public List<User> Users { get; set; } = new();


        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        [BindProperty]
        public User GivenUser { get; set; }
        public LoginModel(FoodItemDBContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;


        }


        public async Task OnGetAsync()
        {
            Users = await _dbContext.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostUserAuthenticationAsync()
        {
            
            var user = await _userManager.FindByEmailAsync(GivenUser.Email);

            if (user == null) {
                Console.WriteLine("------------------USER IS NULL!! -------------");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user, GivenUser.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToPage("Foods");
            }
           
            Console.WriteLine("-------------------USER FOUND BUT WRONG CREDENTIALS ---------------- ");
            return Page();
        }
    }
}













