namespace UTechLeague24.Backend.Auth.Models;

public record AuthenticationResult
{
    public bool Succeeded { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public string? Token { get; set; }
}