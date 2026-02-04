using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace project1.Domains.ItemsDomain.FoodItems
{
    public class CartItem
    {

        public CartItem() { } // EF CORE

        public CartItem(string CartId, string _FoodName, string _FoodDescription, double _FoodPrice, double _FoodTimeToMake, string _FoodDeliverer, string _FoodDeliveryAddress)
        {
            this.CartId = CartId;
            FoodName = _FoodName;
            FoodDescription = _FoodDescription;
            FoodPrice = _FoodPrice;
            FoodTimeToMake = _FoodTimeToMake;
            FoodDeliverer = _FoodDeliverer;
            FoodDeliveryAddress = _FoodDeliveryAddress;
        }




        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodId { get; set; } // unique to the food



        public string? CartId { get; set; } // unique to cart

        
        public int _FoodIdQ { get; set; }

        public int CountOfItem { get; set; } = 1;
        public string? FoodName { get; set; }
        public string? FoodDescription { get; set; }
        public double FoodPrice { get; set; }
        public double? FoodTimeToMakeTheDelivery { get; set; }

        public double FoodTimeToMake { get; set; }

        public string? FoodDeliverer { get; set; } 

        public string? FoodDeliveryAddress { get; set; }


        public string? Seller {  get; set; }

        public int Id { get; set; }
        [ForeignKey("Id")]
        public Cart Cart { get; set; } 
        

        public static CartItemDTO MakeIntoDTO(CartItem cartItem)
        {
            var cartItemDTO = new CartItemDTO { 
                CartId = cartItem.CartId,
                Id = cartItem.FoodId,
                Name = cartItem.FoodName,
                Seller = cartItem.Seller
            };



            return cartItemDTO;
        }


    }
}
