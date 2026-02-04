using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class RegistrationCompleteModel : PageModel
    {
        public FoodItemDBContext _dbContext;
        public List<User> Users { get; set; } = new();
        public string givenToken { get; set; }
        public UserManager<User> _userManager;
        private readonly ILogger<RegistrationCompleteModel> _logger;
        [BindProperty]
        public User GivenUser { get; set; }
        public RegistrationCompleteModel(FoodItemDBContext dbContext, UserManager<User> userManager, ILogger<RegistrationCompleteModel> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostRegistrationCompleteAsync(string Token)
        {

            // if emailconfirmation is enabled
            if(givenToken != Token)
            {
                _logger.LogCritical($"{givenToken} is the entered token and {Token} is the token should have been given.");
                return Page();
            }
            var createdUser = await _userManager.CreateAsync(GivenUser, GivenUser.Password);

            if (!createdUser.Succeeded)
            {
                Console.WriteLine("----------DIDNT SUCCEED BECAUES OF::-------------------------");
                foreach (var error in createdUser.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            var user = await _userManager.FindByEmailAsync(GivenUser.Email);

            if (user != null)
            {


                Console.WriteLine($"----------------The created user has email of {user.Email}-------------");
            }
            else
            {
                Console.WriteLine("--------------------------Created USER COULD NOT BE FOUND!!-----------------------------");
            }

            if (!createdUser.Succeeded)
            {
                foreach (var error in createdUser.Errors)
                {
                    Console.WriteLine($"ERROR: {error.Code} - {error.Description}");
                }

                return Page();
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToPage("/Login");
        }
    }
    }
