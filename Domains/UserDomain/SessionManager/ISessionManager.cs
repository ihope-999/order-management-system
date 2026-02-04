namespace project1.Domains.UserDomain.SessionManager
{
    public interface ISessionManager
    {

        public string GetOrCreateSessionId();


        public ISession GetSession();
        
    }
}
