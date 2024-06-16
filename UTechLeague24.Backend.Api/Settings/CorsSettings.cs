namespace UTechLeague24.Backend.Api.Settings;

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; }
    public string[] AllowedMethods { get; set; }
    public string[] AllowedHeaders { get; set; }
    public bool AllowCredentials { get; set; }
}