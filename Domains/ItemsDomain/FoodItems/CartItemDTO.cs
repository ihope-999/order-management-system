using System.ComponentModel.DataAnnotations;

namespace project1.Domains.ItemsDomain.FoodItems
{
    public class CartItemDTO
    {
        public CartItemDTO()
        {

        }

        [Key]
        public int Id { get; set; } // want to change this to string later 

        public string Name { get; set; }
        public string CartId { get; set; } 

        public string Seller {  get; set; }
    }
}
