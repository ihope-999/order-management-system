using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using project1.Domains.UserDomain.User;

namespace project1.Domains.PaymentDomain.PaymentItem
{
    public class PaymentInfo
    {
        public PaymentInfo(string shippingAddress, string paymentProcessor, string creditCardInfo)
        {
            ShippingAddress = shippingAddress;
            PaymentProcessor = paymentProcessor;
            CreditCardInfo = creditCardInfo;
          
        }
        public PaymentInfo() { }


        public string UserId { get; set; }

        [ForeignKey("Id")]
        public User User { get; set; }


        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int paymentInfoId { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentProcessor {  get; set; }
        public string CreditCardInfo { get; set; }

    }
}
