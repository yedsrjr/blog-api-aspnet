namespace Blog;

public static class Configuration
{

    public static string JwtKey = "YTk4NzY1NDMyMWFiY2RlZjEyMzQ1Njc4OWFiY2Rl";
    public static string ApiKeyName  = "api_key";
    public static string ApiKey  = "curso_api_IlTevUM/z0ey3NwCV/unWg==";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}