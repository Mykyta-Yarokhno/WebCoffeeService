using Microsoft.AspNetCore.Identity;

namespace WebCoffee.Service.Common.Auth
{
    public static class AuthExtensions
    {
        public static async void UseDefaultCoffeeRoles(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleInitializer.InitializeAsync(userManager, rolesManager);
            }


        }


    }
}
