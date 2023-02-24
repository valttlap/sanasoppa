using Sanasoppa.API.Entities;

namespace Sanasoppa.API.DTOs
{
    public class PlayerDto
    {
        public string Name { get; set; } = default!;
        public bool IsDasher { get; set; }
    }
}
