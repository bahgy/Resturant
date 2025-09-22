using Restaurant.BLL.Service.Abstraction;
using Restaurant.BLL.Service.Implementation;
using Restaurant.BLL.Services;
using Restaurant.PL.Filters;
var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// service filtering
builder.Services.AddScoped<ValidateUserExistsFilter>();


// Add Razor Pages services (required if call app.MapRazorPages())
builder.Services.AddRazorPages();

#region Connection string
var connectionString = builder.Configuration.GetConnectionString("Hamza");
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

//////////////////////////////////////////////////////////////
#region identity and authentication

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
#endregion

//////////////////////////////////////////////////////////////
#region automMapper
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
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMenuService, MenuService>();

/// repos
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IMenuRepo, MenuRepo>();

// team
builder.Services.AddScoped<IBookingRepo, BookingRepo>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ITableRepo, TableRepo>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
builder.Services.AddScoped<IEmailNotificationRepo, EmailNotificationRepo>();
builder.Services.AddScoped<IFeedbackRepo, FeedbackRepo>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IPromoCodeRepo, PromoCodeRepo>();
builder.Services.AddScoped<IPromoCodeService, PromoCodeService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IPromoCodeService, PromoCodeService>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<ICartService, CartService>();


builder.Services.AddAutoMapper(x => x.AddProfile(new OrderItemProfile()));
builder.Services.AddAutoMapper(x => x.AddProfile(new OrderProfile()));
builder.Services.AddAutoMapper(x => x.AddProfile(new PromoCodeProfile()));
builder.Services.AddAutoMapper(x => x.AddProfile(new UserProfile()));

#endregion

//////////////////////////////////////////////////////
#region Email Sender
// email sender service
builder.Services.AddTransient<EmailSender>();
#endregion

///////////////////////////////////////////////////////

var app = builder.Build();

#region Middleware
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
#endregion
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
