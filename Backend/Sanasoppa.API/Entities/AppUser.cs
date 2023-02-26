using Microsoft.AspNetCore.Identity;

namespace Sanasoppa.API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public bool HasDefaultPassword { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
