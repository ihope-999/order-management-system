
namespace project1.Domains.UserDomain.SessionManager
{
    public class SessionManager : ISessionManager
    {



        private readonly IHttpContextAccessor _httpContextAccessor;



        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetOrCreateSessionId()
        {
            var session = GetSession();
            var mySessionId = session.GetString("MySessionId");
            if(mySessionId == null)
            {
                mySessionId = Guid.NewGuid().ToString();
                session.SetString("MySessionId", mySessionId);
            }

            return mySessionId;


        }

        public ISession GetSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            return session;
        }

       
    }
}
