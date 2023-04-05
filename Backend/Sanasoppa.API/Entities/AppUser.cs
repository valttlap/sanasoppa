using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Sanasoppa.API.Entities;
public partial class AppUser
{
    [Required]
    [EmailAddress]
    public override string? Email { get; set; }
    [Required]
    public override string? UserName { get; set; }
}

public partial class AppUser : IdentityUser<int>
{
    public AppUser()
    {
        UserRoles = new List<AppUserRole>();
    }
    public ICollection<AppUserRole> UserRoles { get; set; }
}
