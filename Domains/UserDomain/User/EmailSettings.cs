using System.Security.Cryptography.X509Certificates;

namespace project1.Domains.UserDomain.User
{
    public class EmailSettings
    {

        public EmailSettings() { }
        public int SmtpPort { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpSender { get; set; }
        public string SmtpPassword { get; set; }    
        
    }
}
