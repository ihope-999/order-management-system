using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class RequestForAdminRoleModel : PageModel
    {
        private readonly ILogger<RequestForAdminRoleModel> _logger;
        private readonly UserManager<User> _userManager;

        private readonly FoodItemDBContext _dbContext;
        public RequestUser RequestUser { get; set; } = new();
        public RequestForAdminRoleModel(ILogger<RequestForAdminRoleModel> logger, UserManager<User> userManager, FoodItemDBContext dBContext)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dBContext;
        }


        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Page();

            }
            _logger.LogInformation("--------------YOU ARE NOT LOGGED IN TO ACCESS TO THE SITE ---------------");
            return RedirectToPage("Index");


        }

        public async Task<IActionResult> OnPostEndTheProcessAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                _logger.LogWarning("-----COULDNT END THE PROCESS OF ADMIN ROLE REQUEST BECAUSE OF THE USER BEING NULL --------------");
                return RedirectToPage("Index");
            }
            RequestUser.RoleRequest = "Admin";
            RequestUser.Id = user.Id;
            RequestUser.RealUserName = user.RealUserName;
            RequestUser.FirstName = user.FirstName;
            _dbContext.RequestUsers.Add(RequestUser);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("Foods");
        }
    }
}
