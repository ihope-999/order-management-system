using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace project1.Domains.UserDomain.User
{
    public class RequestUser
    {
        public RequestUser() { }
        public string RealUserName { get; set; }
        public string? FirstName {  get; set; }

        [Key]
        public string Id { get; set; }
        public string RoleRequest { get; set; }
    }
}
