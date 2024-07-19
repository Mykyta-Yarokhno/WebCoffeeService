using Microsoft.AspNetCore.Identity;

namespace WebCoffee.Service.Common.Auth
{
    public class Roles
    {
        public const string Admin = "admin";
        public const string Employee = "employee";
    }

    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            if(await roleManager.FindByNameAsync(Roles.Admin) == null)
            {
               await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if( await roleManager.FindByNameAsync(Roles.Employee) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Employee));
            }

            string defaultAdminEmail = "coffee.admin@gmail.com";

            if (await userManager.FindByNameAsync(defaultAdminEmail) == null)
            {
                var admin = new IdentityUser { Email = defaultAdminEmail, UserName = defaultAdminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, "_Aa123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin,Roles.Admin);
                }
            }
        }
    }

}
