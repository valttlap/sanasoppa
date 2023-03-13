using Microsoft.AspNetCore.Identity;

namespace Sanasoppa.API.Entities;
public class AppRole : IdentityRole<int>
{
    public AppRole() 
    {
        UserRoles = new List<AppUserRole>();
    }
    public ICollection<AppUserRole> UserRoles { get; set; }
}
