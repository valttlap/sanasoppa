using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data
{
    public class Seed
    {
        public static async Task SeedDefaultUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
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
                HasDefaultPassword = true,
            };

            var result = await userManager.CreateAsync(admin, "SanaSoppa2023!");

            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
                await userManager.UpdateSecurityStampAsync(admin);
            }
        }

    }
}
