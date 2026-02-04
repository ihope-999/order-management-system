using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using project1.Domains.UserDomain.SessionManager;
using project1.Domains.UserDomain.User;
namespace project1.Domains.UserDomain.Handlers
{
    public class RegisterUserHandler : IRegisterUserHandler
    {

        private readonly UserManager<project1.Domains.UserDomain.User.User> _userManager;
        private readonly ISessionManager _sessionManager;
        public RegisterUserHandler(UserManager<project1.Domains.UserDomain.User.User> userManager, ISessionManager sessionManager)
        {

            _userManager = userManager;
            _sessionManager = sessionManager;

        }


        public async Task RegisterUser()
        {

            throw new NotImplementedException();
        }

        public async Task<bool> CheckLoginStatus(ClaimsPrincipal User)
        {
            return User.Identity.IsAuthenticated ? true : false;
        }

    }
}