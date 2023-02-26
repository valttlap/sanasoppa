using Microsoft.AspNetCore.Identity;

namespace Sanasoppa.API.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUserRole(AppUser user, AppRole role)
        {
            User = user;
            Role = role;
        }
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
