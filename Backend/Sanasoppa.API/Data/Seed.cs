using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data
{
    public class Seed
    {
        public static async Task SeedDefaultUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, bool isDevelopment)
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

            if (isDevelopment)
            {
                var defaultUsers = new List<AppUser>
                {
                    new AppUser{UserName = "user1", HasDefaultPassword = true},
                    new AppUser{UserName = "user2", HasDefaultPassword = true},
                    new AppUser{UserName = "user3", HasDefaultPassword = true},
                    new AppUser{UserName = "user4", HasDefaultPassword = true},
                    new AppUser{UserName = "user5", HasDefaultPassword = true},
                    new AppUser{UserName = "user6", HasDefaultPassword = true},
                    new AppUser{UserName = "user7", HasDefaultPassword = true},
                    new AppUser{UserName = "user8", HasDefaultPassword = true},
                    new AppUser{UserName = "user9", HasDefaultPassword = true},
                    new AppUser{UserName = "user10", HasDefaultPassword = true},
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
}
