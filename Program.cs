using System;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using project1.Domains.ItemsDomain.Database;
using project1.Domains.ItemsDomain.Interface;
using project1.Domains.ItemsDomain.Services.FoodHandlers;
using project1.Domains.UserDomain.SessionManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using project1.Domains.UserDomain.User;
using System.Linq.Expressions;
using project1.Domains.UserDomain;
using project1.Domains.UserDomain.Handlers;
using project1.Domains.UserDomain.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IFoodItemHandler, FoodItemHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

/*builder.Services.AddDbContext<FoodItemDBContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});*/

var password = builder.Configuration["YOURPASSWORD"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var emailSettings = builder.Configuration.GetSection("EmailSettings");
builder.Services.Configure<EmailSettings>(emailSettings);

connectionString = connectionString.Replace("{YOURPASSWORD}",password);
builder.Services.AddDbContext<FoodItemDBContext>(options =>
    options.UseNpgsql(connectionString));



builder.Services.AddIdentity<User, IdentityRole>(options =>

{

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;


}).AddEntityFrameworkStores<FoodItemDBContext>().AddDefaultTokenProviders();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});
//Mediatr

builder.Services.AddScoped<ISessionManager, SessionManager>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IRegisterUserHandler, RegisterUserHandler>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); 
    options.Cookie.HttpOnly = true;                
    options.Cookie.IsEssential = true;             
});



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; 
        options.LogoutPath = "/Logout"; 
        options.ExpireTimeSpan = TimeSpan.FromHours(1); 
        options.SlidingExpiration = true;
    });

var app = builder.Build();




builder.Services.AddAuthorization();

/*using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FoodItemDBContext>();
    db.Database.EnsureCreated();
}*/



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<FoodItemDBContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        context.Database.EnsureCreated();

        string[] roles = { "Admin", "User", "Seller", "Courier" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"New role created {role}");
            }
        }
        var adminEmail = "admin@hotmail.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User("admin", "admin", adminEmail, "0000", true, adminEmail);

            Console.WriteLine($"DEBUG: UserName after creation = '{adminUser.UserName}'");
            Console.WriteLine($"DEBUG: Email after creation = '{adminUser.Email}'");

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine("********-----------------------Admin created successfully!------------------------------------");
            }
            else
            {
                Console.WriteLine("------------------------ ADMIN CREATION FAILED -------------");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code} - {error.Description}");
                }
                Console.WriteLine("-----------------------------------------------------------");
            }

        }

    }
    catch (Exception ex)
    {

    }
}
        if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
