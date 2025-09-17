namespace Restaurant.PL.Helpers
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration config)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

    // Roles
    string[] roles = { "Admin", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<int>(role));
    }

    // Admin user from config
    var adminEmail = config["AdminUser:Email"];
    var adminPassword = config["AdminUser:Password"];

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
                adminUser = new Admin
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    UserType = Restaurant.DAL.Enum.UserTypeEnum.Admin,
                    PhoneNumber = "0000000000"
                };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
            else
            {
                // password reset if it changed in appsettings
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, adminPassword);
            }
        }

        }
}
