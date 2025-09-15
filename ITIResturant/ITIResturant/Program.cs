
using BIL.Service.Abstraction;
using BIL.Service.Impelementation;
using DAL.Repo.Abstraction;
using DAL.Repo.Impelementation;

namespace ITIResturant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ResturantDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connect1")));


            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<ITableRepo, TableRepo>();
            builder.Services.AddScoped<ITableService, TableService>();
            builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
            builder.Services.AddScoped<IEmailNotificationRepo, EmailNotificationRepo>();
            builder.Services.AddScoped<IFeedbackRepo, FeedbackRepo>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())

            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
