namespace UTechLeague24.Backend.Auth.Models;

/// <summary>
/// Represents the result of an authentication operation.
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    /// Indicates whether the authentication was successful.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// A collection of error messages if the authentication failed. Will be null or empty if authentication succeeded.
    /// </summary>
    public IEnumerable<string>? Errors { get; set; }

    /// <summary>
    /// The authentication token (JWT) issued upon successful authentication. Will be null if authentication failed.
    /// </summary>
    public string? Token { get; set; }
}