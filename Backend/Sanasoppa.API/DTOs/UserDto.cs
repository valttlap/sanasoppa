﻿namespace Sanasoppa.API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; } = default!;
        public string Token { get; set; } = default!;
        public bool HasDefaultPassword { get; set; }
    }
}
