using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.ItemsDomain.Interface;
using project1.Domains.ItemsDomain.Services;
using project1.Domains.ItemsDomain.Services.FoodHandlers;
using project1.Domains.UserDomain.SessionManager;
using project1.Domains.UserDomain.User;

namespace project1.Pages
{
    public class FoodsModel : PageModel
    {


        private readonly IMediator _mediator;

        public ISessionManager _sessionManager;

        public IFoodItemHandler _foodItemHandler;

        public ILogger<FoodsModel> _logger;
        public bool isLogged { get; set; } = false;
        public FoodItemDBContext _dbContext { get; set; }

        public UserManager<User> _userManager { get; set; }
        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

        public FoodsModel(FoodItemDBContext dbContext, IFoodItemHandler foodItemHandler, IMediator mediator, ISessionManager sessionManager, UserManager<User> userManager,ILogger<FoodsModel> logger)
        {
            _dbContext = dbContext;
            _foodItemHandler = foodItemHandler;
            _mediator = mediator;
            _sessionManager = sessionManager;
            _userManager = userManager;
            _logger = logger;


        }

        [BindProperty]
        public FoodItem addedFoodItem { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }

        public User user;
        public async Task OnGetAsync()
        {




            if ((User.Identity.IsAuthenticated) == true)
            {
                user = await _userManager.FindByEmailAsync(User.Identity.Name);
                UserId = user.Id;

            }
            else
            {
                SessionId = _sessionManager.GetOrCreateSessionId();

            }




            /*if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                
            }*/


            await _dbContext.CreateFakeData();
            Console.WriteLine("The fake data is created!");

            FoodItems = await _dbContext.FoodItems.ToListAsync();

            Console.WriteLine("foods are gotten to the list!");

            Console.WriteLine($"SessionId {SessionId} and userId {UserId}");

         
        }


        public async Task<IActionResult> OnPostAddHandlerAsync()
        {

            //using mediator

            var email = User.Identity.Name;
            var command = new AddFoodCommand(addedFoodItem,email);
            await _mediator.Send(command);



            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteHandlerAsync(int id)
        {
            var command = new DeleteCommand(id);
            await _mediator.Send(command);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddToCartHandlerAsync(int id, string Username)
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                UserId = user.Id;
            }
            else
            {
                SessionId = _sessionManager.GetOrCreateSessionId();

            }

            var itemToAdd = await _dbContext.FoodItems.FindAsync(id);
            var cartItemToAdd = new CartItem
            {
                CartId = UserId ?? SessionId,
                _FoodIdQ = itemToAdd.FoodId,
                FoodDeliverer = itemToAdd.FoodDeliverer,
                FoodDeliveryAddress = itemToAdd.FoodDeliveryAddress,
                FoodName = itemToAdd.FoodName,
                FoodPrice = itemToAdd.FoodPrice,
                FoodDescription = itemToAdd.FoodDescription,
                FoodTimeToMake = itemToAdd.FoodTimeToMake,
                FoodTimeToMakeTheDelivery = itemToAdd.FoodTimeToMakeTheDelivery,
                Seller = itemToAdd.Seller,

            };
            _logger.LogInformation($"The count of the food is {id} and the cookie/session is {UserId}---------------Seller is {cartItemToAdd.Seller}");
            var command = new AddCartCommand(cartItemToAdd,SessionId,UserId);
            await _mediator.Send(command);


            return RedirectToPage();

        }
        public async Task<IActionResult> OnPostUpdateHandlerAsync(int id, string Username)
        {

            return RedirectToPage("/UpdateFood", new {id = id});

        }
    }
}
