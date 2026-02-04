
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.PaymentDomain.PaymentItem;
using project1.Domains.UserDomain.User;

namespace project1.Domains.ItemsDomain.Database

{
    public class FoodItemDBContext : IdentityDbContext<User>
    {



        public FoodItemDBContext (DbContextOptions<FoodItemDBContext> options) : base(options) { }

        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts {  get; set; }
        public DbSet<RequestUser> RequestUsers { get; set; }

        public DbSet <CartItemDTO> cartItemDTOs { get; set; }

     
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<Cart>().
                HasMany<CartItem>(cart => cart.Items).
                WithOne(cartitem => cartitem.Cart).
                HasForeignKey(cartItem => cartItem.Id);



            modelBuilder.Entity<User>().
                HasMany(paymentinfos => paymentinfos.PaymentInfo).
                WithOne(paymentinfo => paymentinfo.User).
                HasForeignKey(paymentinfo => paymentinfo.UserId);


            modelBuilder.Entity<Cart>().
                HasOne<Status>(cart => cart.Status).
                WithOne(status => status.Cart).
                HasForeignKey<Status>(status => status.Id); // one to one so specifying needs to be done about who is dependent dont forget.
        }














     



        public async Task CreateFakeData()
        {
/*            if (!FoodItems.Any())
            {
                var newFood = new FoodItem("Pizza", "Description", 3, 4, "Deliverer", "Address1");
                var newFood2 = new FoodItem("A", "Description", 3, 4, "Deliverer", "Address1");
                await FoodItems.AddRangeAsync(newFood, newFood2);
                await SaveChangesAsync();
            }
             */

        
    }

        
    }

    

 
}
