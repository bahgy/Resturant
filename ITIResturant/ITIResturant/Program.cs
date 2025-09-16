using Microsoft.Extensions.DependencyInjection;
using Restaurant.BLL.Abstraction;
using Restaurant.BLL.Service.Impelementation;
using Restaurant.BLL.Services.Interfaces;
using Restaurant.PL.Helpers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Add Razor Pages services (required if call app.MapRazorPages())
builder.Services.AddRazorPages();

// Connection string
var connectionString = builder.Configuration.GetConnectionString("Hamza");
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlServer(connectionString));

//////////////////////////////////////////////////////////////

// identity and authentication

builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    // allow login without confirmed email
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<RestaurantDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddAuthentication()
    .AddGoogle(options =>

    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });

// remember me cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});

//////////////////////////////////////////////////////////////
// auto mapper
builder.Services.AddAutoMapper(typeof(DomainProfile));
///
//// dependecy injection  ///////////////////////////////////
/// services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IUserService, UserService>();
/// repos
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
//////////////////////////////////////////////////////
// email sender service
builder.Services.AddTransient<EmailSender>();


///////////////////////////////////////////////////////

var app = builder.Build();

    // Middleware
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
    }
    else
    {
        // In Development can still route status codes to your error controller
        // default error page
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
    }

// Seed default data like admin user and roles if not exists in the database 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var config = services.GetRequiredService<IConfiguration>();
    await SeedData.InitializeAsync(services, config);
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
