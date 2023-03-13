using Microsoft.AspNetCore.Identity;

namespace Sanasoppa.API.Entities;
public class AppUserRole : IdentityUserRole<int>
{
    public AppUser User { get; set; } = default!;
    public AppRole Role { get; set; } = default!;
}
