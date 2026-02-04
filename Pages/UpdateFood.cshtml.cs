using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.ItemsDomain.Services;
using project1.Domains.ItemsDomain.Services.FoodHandlers;

namespace project1.Pages
{
    public class UpdateFoodModel : PageModel
    {
        
        [BindProperty]
        public FoodItem updateFood { get; set; }
        public FoodItemDBContext _dbContext;
        public IMediator _mediator { get; set; }
        public UpdateFoodModel (FoodItemDBContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task OnGetAsync(int id) 
        {
            Console.WriteLine($"The food you want to change is {id}");
            updateFood = await _dbContext.FoodItems.FindAsync(id);
        }

        public async Task<IActionResult> OnPostUpdateHandlerAsync()
        {
        
            var command = new UpdateCommand(updateFood);
         
            await _mediator.Send(command);
            Console.WriteLine("The update request is sent!");
     

            return RedirectToPage("Foods");

        }



    }
}
