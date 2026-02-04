using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace project1.Domains.UserDomain
{
    public interface IRegisterUserHandler
    {

        public Task RegisterUser();
        public Task<bool> CheckLoginStatus(ClaimsPrincipal User);


    }
}
