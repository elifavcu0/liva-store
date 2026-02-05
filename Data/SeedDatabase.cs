using Microsoft.AspNetCore.Identity;

namespace dotnet_store.Data;

public static class SeedDatabase
{
    public static async void Initialize(IApplicationBuilder app)
    {
        var userManager = app.ApplicationServices.CreateAsyncScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = app.ApplicationServices.CreateAsyncScope().ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        if (!roleManager.Roles.Any())
        {
            var admin = new AppRole { Name = "Admin" };
            await roleManager.CreateAsync(admin);
        }

        if (!userManager.Users.Any())
        {
            var userAdmin = new AppUser
            {
                NameSurname = "Elif Avcu",
                UserName = "elifavcu",
                Email = "elif@gmail.com",
            };
            await userManager.CreateAsync(userAdmin, "123456");
            await userManager.AddToRoleAsync(userAdmin, "Admin");
        }
    }
}