namespace Sanasoppa.Application.DTOs;
public class ExplanationDto
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = default!;
    public string Explanation { get; set; } = default!;
}
