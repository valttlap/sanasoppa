namespace Sanasoppa.API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public bool HasDefaultPassword { get; set; }
    }
}
