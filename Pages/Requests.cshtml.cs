using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class RequestsModel : PageModel
    {
        private readonly FoodItemDBContext _dbContext;
        private readonly ILogger<RequestsModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ICollection<RequestUser> RequestUsers { get; set; } = new List<RequestUser>();

        public RequestsModel(ILogger<RequestsModel> logger, FoodItemDBContext dBContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _dbContext = dBContext;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task OnGetAsync()
        {
            RequestUsers = await _dbContext.RequestUsers.ToListAsync();

        }

        public async Task OnPostGiveRoleAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("-----------------USER ROLE PERMISSION COULD NOT BE FOUND SINCE USER NULL-------------");
            }
            var requestUser = await _dbContext.RequestUsers.FindAsync(user.Id);
            var result = await _userManager.AddToRoleAsync(user, requestUser.RoleRequest);
            if (result.Succeeded)
            {
                _logger.LogInformation($"---------------THE ADMIN REQUEST OF {user.RealUserName} is ACCEPTED!-------------------");
                _dbContext.RequestUsers.Remove(requestUser);
                var found = await _dbContext.RequestUsers.FindAsync(user.Id);
                if(found != null)
                {
                    _logger.LogWarning("The REMOVAL COULD NOT BE DONE FOR THE REQUESTUSER");
                }
                
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogWarning($"-----------------------Error WHEN ASSIGNING ROLE: {error}: {error.Description}-------------------");
                }

            }
           
            await _dbContext.SaveChangesAsync();

        }
    }
}
