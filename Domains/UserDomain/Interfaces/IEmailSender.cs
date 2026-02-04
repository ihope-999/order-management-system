namespace project1.Domains.UserDomain.Interfaces
{
    public interface IEmailSender
    {
        public void sendEmail(string toEmail, string Subject, string Body, string Token, bool enabled);

    }
}
