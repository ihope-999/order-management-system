using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using project1.Domains.PaymentDomain.PaymentItem;







namespace project1.Domains.UserDomain.User
{

    public class User : IdentityUser
    {
        public User() { }

        public User(string FirstName , string LastName , string email, string TelNum, bool emailConfirmed, string userName) {

            this.FirstName = FirstName;
            this.LastName = LastName;
            Email = email;
            this.TelNum = TelNum;
            EmailConfirmed = emailConfirmed;
            UserName = email;
        }

        
        public string? RealUserName { get; set; }

        public string? Password { get; set; }
        public string UserId {  get; set; } = Guid.NewGuid().ToString();

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? TelNum { get; set; }

        public string isAdmin { get; set; } = "false";

        public ICollection<PaymentInfo> PaymentInfo { get; set; } = new List<PaymentInfo>();


        public string FullName => $"{FirstName} +  {LastName}";


    }

}




  /*  namespace project1.Domains.UserDomain.User
{
    public class User
    {
        public User() { }

        public User(string UserName, string Password, string Email, string TelNum)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Email = Email;
            this.TelNum = TelNum;

        }

        
        public string Id { get; set; }

        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString(); 
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? TelNum { get; set; }

        public string IsAdmin { get; set; } = "false";

        public List<PaymentInfo> PaymentInfo { get; set; } = new List<PaymentInfo>();






    }
}

*/