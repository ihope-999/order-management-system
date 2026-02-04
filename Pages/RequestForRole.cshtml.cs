using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace project1.Pages
{
    public class RequestToAdminModel : PageModel
    {
        private readonly ILogger<RequestToAdminModel> _logger;

        [BindProperty]
        public string RoleRequest { get; set; }

        public RequestToAdminModel(ILogger<RequestToAdminModel> logger)
        {
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("The user is not logged in for this request");
                return RedirectToPage("Index");
            }

            return Page();
        }

        public IActionResult OnPostRoleAssignmentRequest(string role)
        {
            if(role == "Admin")
            {
                
                _logger.LogInformation($"the request: {role} --------------You want to request the role of ADMIN---------------");
                return RedirectToPage("RequestForAdminRole");
            }
            
            _logger.LogInformation($"the request: {role} --------------You want to request the role of COURIER---------------");
            return RedirectToPage("RequestForCourierSellerRole", new {Role = role});
            
           
            
            

        }
    }
}
