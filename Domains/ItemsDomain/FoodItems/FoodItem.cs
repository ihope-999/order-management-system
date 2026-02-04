using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project1.Domains.ItemsDomain.FoodItems
{
    public class FoodItem
    {

        public FoodItem() { } // EF CORE

        public FoodItem(string _FoodName, string _FoodDescription, double _FoodPrice, double _FoodTimeToMake, string _FoodDeliverer, string _FoodDeliveryAddress)
        {
            FoodName = _FoodName;
            FoodDescription = _FoodDescription;
            FoodPrice = _FoodPrice;
            FoodTimeToMake = _FoodTimeToMake;
            FoodDeliverer = _FoodDeliverer;
            FoodDeliveryAddress = _FoodDeliveryAddress;
        }


        [Key]
        public int FoodId { get; set; }
        public string? FoodName { get; set; }
        public string? FoodDescription { get; set; }
        public double FoodPrice { get; set; }
        public double? FoodTimeToMakeTheDelivery { get; set; }

        public double FoodTimeToMake { get; set; }

        public string? FoodDeliverer { get; set; }

        public string? FoodDeliveryAddress { get; set; }
        public string Seller {  get; set; }



    }
}
