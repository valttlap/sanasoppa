using Microsoft.AspNetCore.Identity;

namespace Sanasoppa.API.Entities;
public class AppUser : IdentityUser<int>
{
    public AppUser()
    {
        UserRoles = new List<AppUserRole>();
    }
    public bool HasDefaultPassword { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
}
