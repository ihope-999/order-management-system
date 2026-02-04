using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                Console.WriteLine("The user was logged in, this action LOGS OUT!");

            }
            else
            {
                Console.WriteLine("You are not logged in!");
            }
            await _signInManager.SignOutAsync();

        }
    }
}
