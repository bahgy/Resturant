    using System.Globalization;
    using System.Security.Claims;
    using Hangfire;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Restaurant.BLL.Service.Abstraction;
    using Restaurant.BLL.Service.Implementation;
    using Restaurant.BLL.Services;
    using Restaurant.DAL.Repo.Implementation;
    using Restaurant.DAL.Repo.Abstraction;
    using Restaurant.PL;
    using Restaurant.PL.chatHub;
    using Restaurant.PL.Filters;
    using Restaurant.PL.Language;
    using Resturant.BLL.Service.Abstraction;
    using Resturant.BLL.Service.Impelementation;
    using Rsturant.DAL.Repo.Abstraction;
    using Rsturant.DAL.Repo.Impelementation;

    var builder = WebApplication.CreateBuilder(args);

    // ------------------------
    // MVC + Localization
    // ------------------------
    builder.Services.AddControllersWithViews()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (type, factory) =>
                factory.Create(typeof(SharedResource));
        });

    builder.Services.AddRazorPages();
    builder.Services.AddHttpContextAccessor();

    // ------------------------
    // Database
    // ------------------------
    var connectionString = builder.Configuration.GetConnectionString("connection");
    builder.Services.AddDbContext<RestaurantDbContext>(options =>
        options.UseSqlServer(connectionString));

    // ------------------------
    // Identity & Authentication
    // ------------------------
    builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
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

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
    });

    // ------------------------
    // AutoMapper (single registration)
    // ------------------------
    builder.Services.AddAutoMapper(typeof(DomainProfile), typeof(AccountProfile));

    // ------------------------
    // SignalR
    // ------------------------
    builder.Services.AddSignalR(); // add hub options here if needed

    // ------------------------
    // Hangfire
    // ------------------------
    builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
    builder.Services.AddHangfireServer(options => { options.WorkerCount = 1; });

    // ------------------------
    // App services & repos
    // ------------------------
    // (keep the ones you actually need; deduplicated duplicate entries)
    builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
    builder.Services.AddScoped<IReviewService, ReviewService>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IProfileService, ProfileService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IMenuService, MenuService>();
    builder.Services.AddScoped<IProductRepo, ProductRepo>();
    builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
    builder.Services.AddScoped<IAdminRepo, AdminRepo>();
    builder.Services.AddScoped<IMenuRepo, MenuRepo>();
    builder.Services.AddScoped<IBookingRepo, BookingRepo>();
    builder.Services.AddScoped<IBookingService, BookingService>();
    builder.Services.AddScoped<ITableRepo, TableRepo>();
    builder.Services.AddScoped<ITableService, TableService>();
    builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
    builder.Services.AddScoped<IEmailNotificationRepo, EmailNotificationRepo>();
    builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
    builder.Services.AddScoped<IOrderRepo, OrderRepo>();
    builder.Services.AddScoped<IPromoCodeRepo, PromoCodeRepo>();
    builder.Services.AddScoped<IPromoCodeService, PromoCodeService>();
    builder.Services.AddScoped<IOrderItemService, OrderItemService>();
    builder.Services.AddScoped<ICartRepo, CartRepo>();
    builder.Services.AddScoped<ICartService, CartService>();
    builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
    builder.Services.AddScoped<IPaymentService, PaymentService>();
    builder.Services.AddScoped<IDeliveryRepo, DeliveryRepo>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IMessageService, MessageService>();
    builder.Services.AddScoped<IChatRepo, ChatRepo>();

    // ------------------------
    // IUserIdProvider (single registration)
    // ------------------------
    // Choose one provider depending on which claim you set for users.
    // I chose NameUserIdProvider which uses ClaimTypes.NameIdentifier.
    // Ensure your authentication issues ClaimTypes.NameIdentifier (e.g. user id).
    builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

    // If you prefer to use a different claim name (e.g. "Id") implement and register it here instead.
    // builder.Services.AddSingleton<IUserIdProvider, AppUserIdProvider>();

    // ------------------------
    // Misc
    // ------------------------
    builder.Services.AddScoped<ValidateUserExistsFilter>();
    builder.Services.AddTransient<EmailSender>();

    var app = builder.Build();

    // ------------------------
    // Error handling
    // ------------------------
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
    }
    else
    {
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
    }

    // ------------------------
    // Seed data (keeps your existing pattern)
    // ------------------------
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();
        await SeedData.InitializeAsync(services, config);
    }



    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    // IMPORTANT: order of auth middleware
    app.UseAuthentication();
    app.UseAuthorization();

    // Hangfire dashboard
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthorizationFilter() },
        StatsPollingInterval = 10000
    });

    // Localization
    var supportedCultures = new[] { new CultureInfo("ar-EG"), new CultureInfo("en-US") };
    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures,
        RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        }
    });

    // Routes
    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages();

    // --------- Map SignalR Hub ---------
    // Ensure the path matches your client call: .withUrl("/chatHub")
    app.MapHub<ChatHub>("/chatHub");

    app.Run();
