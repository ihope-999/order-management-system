using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project1.Domains.ItemsDomain.FoodItems
{
    public class Status
    {

        public Status() { }



        [Key]
        public int Id { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsAcceptedByTheDelivery { get; set; } = false;

        public bool isOrdered { get; set; } = false;
        public Cart Cart { get; set; }
        public string? CartId { get; set; }

    }

}
