using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data;
public static class Seed
{
    public static async Task SeedDefaultUserAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, bool isDevelopment)
    {
        if (await userManager.Users.AnyAsync()) return;

        var roles = new List<AppRole>
        {
            new AppRole{Name = "Member"},
            new AppRole{Name = "Admin"},
            new AppRole{Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        var admin = new AppUser
        {
            UserName = "admin",
            Email = "admin@sanasoppa.app",
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(admin, "SanaSoppa2023!");

        if (result.Succeeded)
        {
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
            await userManager.UpdateSecurityStampAsync(admin);
        }

        if (isDevelopment)
        {
            var defaultUsers = new List<AppUser>
            {
                new AppUser{UserName = "user1", Email = "user1@sanasoppa.app", EmailConfirmed = true},
                new AppUser{UserName = "user2", Email = "user2@sanasoppa.app", EmailConfirmed = true},
                new AppUser{UserName = "user3", Email = "user3@sanasoppa.app", EmailConfirmed = true},
                new AppUser{UserName = "user4", Email = "user4@sanasoppa.app", EmailConfirmed = true}
            };

            foreach (var user in defaultUsers)
            {
                var createResult = await userManager.CreateAsync(user, "SanaSoppa2023!");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Member");
                    await userManager.UpdateSecurityStampAsync(user);
                }
            }
        }
    }

}
