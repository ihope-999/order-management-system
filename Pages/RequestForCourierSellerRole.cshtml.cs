using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class RequestForCourierSellerRoleModel : PageModel
    {

        private readonly FoodItemDBContext _dbContext;
        private readonly ILogger<RequestForCourierSellerRoleModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RequestUser RequestUser { get; set; } = new();
        public string role {  get; set; }


        public RequestForCourierSellerRoleModel(ILogger<RequestForCourierSellerRoleModel> logger, FoodItemDBContext dBContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _dbContext = dBContext;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task OnGetAsync(string Role)
        {
            role = Role;  
        }

        public async Task<IActionResult> OnPostCourierRoleAssignmentRequestAsync()
        {

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                _logger.LogWarning("-----COULDNT END THE PROCESS OF ADMIN ROLE REQUEST BECAUSE OF THE USER BEING NULL --------------");
                return RedirectToPage("Index");
            }
            RequestUser.RoleRequest = "Courier";
            RequestUser.Id = user.Id;
            RequestUser.RealUserName = user.RealUserName;
            RequestUser.FirstName = user.FirstName;
            _dbContext.RequestUsers.Add(RequestUser);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("A user applied for Courier position");


            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostSellerRoleAssignmentRequestAsync()
        {

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                _logger.LogWarning("-----COULDNT END THE PROCESS OF ADMIN ROLE REQUEST BECAUSE OF THE USER BEING NULL --------------");
                return RedirectToPage("Index");
            }

            RequestUser.RoleRequest = "Seller";
            RequestUser.Id = user.Id;
            RequestUser.RealUserName = user.RealUserName;
            RequestUser.FirstName = user.FirstName;
            _dbContext.RequestUsers.Add(RequestUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("A user applied for Seller position");
            return RedirectToPage("Index");
        }
    }
}
