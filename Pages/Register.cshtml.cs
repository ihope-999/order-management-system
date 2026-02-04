using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.FoodItems;
using project1.Domains.UserDomain.User;
using System.Net.Mail;
using project1.Domains.UserDomain.Interfaces;
using project1.Domains.UserDomain.Handlers;

namespace project1.Pages
{
    public class RegisterModel : PageModel
    {
        public FoodItemDBContext _dbContext;
        public List<User> Users { get; set; } = new();

        public UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public User GivenUser { get; set; }
        public RegisterModel(FoodItemDBContext dbContext, UserManager<User> userManager, ILogger<RegisterModel> logger, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            bool emailConfirmation = false; // decides the confirming process whether you want to use email in conf or not

            _logger.LogInformation($"----------------------The properties are {GivenUser.Email}-----------------------------------------");

            
            GivenUser.UserName = GivenUser.Email;
            var result = await UserNecessaryChecksAsync(GivenUser);
            if(!result) return Page();
            string token = createRegisterNumber(6);
            _logger.LogCritical(token);




            if (emailConfirmation)
            {
                _emailSender.sendEmail(GivenUser.Email, "Registration", "You are now registered", token, emailConfirmation);

                return RedirectToPage("RegistrationComplete", new { Token = token });
            }








            /* ---------------------------------IF YOU DO NOT WANT TO EMAIL CONFIRM THEN ----------------------------------*/ 
            var createdUser = await _userManager.CreateAsync(GivenUser, GivenUser.Password);

            if (!createdUser.Succeeded)
            {
                _logger.LogInformation("----------DIDNT SUCCEED BECAUES OF::-------------------------");
                foreach (var error in createdUser.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            var user = await _userManager.FindByEmailAsync(GivenUser.Email);

            if (user != null)
            {


                _logger.LogInformation($"----------------The created user has email of {user.Email}-------------");
            }
            else
            {
                _logger.LogInformation("--------------------------Created USER COULD NOT BE FOUND!!-----------------------------");
            }

            if (!createdUser.Succeeded)
            {
                foreach (var error in createdUser.Errors)
                {
                    _logger.LogInformation($"ERROR: {error.Code} - {error.Description}");
                }

                return Page();
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToPage("/Login");
        
        }

        public async Task<bool> PasswordStrengthCheckAsync(User user, UserManager<User> _userManager, string password)
        {


            try
            {
                var validator = new PasswordValidator<User>();
                var result = await validator.ValidateAsync(_userManager, user, password);
                if (result.Succeeded) return true;
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogCritical($"{error}: {error.Description}");
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error occured while checking password strength: {ex.Message}");
                return false;
            }
            

        }

        public async Task<bool> EmailCheck(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var result = new MailAddress(email);
                if (user != null)
                {
                    _logger.LogCritical("The email already is being used by another user");
                    return false;
                }
                return result.Address == email;
            }
            catch (Exception ex) 
            {
                _logger.LogCritical($"Error occured while checking email: {ex.Message}");
                return false;
                
            }

        }

        public async Task<bool> UserNecessaryChecksAsync(User GivenUser)
        {
            var passwordResult = await PasswordStrengthCheckAsync(GivenUser, _userManager, GivenUser.Password);
            var emailResult = await EmailCheck(GivenUser.Email);
            if (!passwordResult) return false;
            if (!emailResult) return false ;
            return true;

            

        }

        public string createRegisterNumber(int length = 6)
        {
            string number = "";
            var random = new Random();
            for(int i = 0; i< length; i++)
            {
                number += random.Next(0, 10);
            }
            return number;
        }
        /*Validator public PasswordStrength PasswordStrengthCheck(string Password)
        {
            int score = 0;
            bool passed = false;
            if (Password == null) return new PasswordStrength(0, false);
            if (Regex.IsMatch(Password, "[a-z]")) score++;
            if (Regex.IsMatch(Password, "[A-Z]")) score++;
            if (Regex.IsMatch(Password, "[^a-zA-Z0-9]")) score++; //special characters

            
            if (Regex.IsMatch(Password, "[A-Z]") &&
            Regex.IsMatch(Password, "[^a-zA-Z9-9]"))
            {
                passed = true;
            }

            var result = new PasswordStrength(score, passed);
            return result;

        }*/
    }
}
