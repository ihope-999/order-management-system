using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project1.Domains.ItemsDomain.FoodItems
{
    public class Cart
    {
        public Cart() {}



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int paymentInfoId { get; set; }
        
        public string CartId { get; set; } = Guid.NewGuid().ToString();
        public List<CartItem> Items { get; set; } = new();

        public Status Status { get; set; } = new();

    }
}
