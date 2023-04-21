namespace Sanasoppa.API.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; } = default!;
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public int UserId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public bool Revoked { get; set; }
}