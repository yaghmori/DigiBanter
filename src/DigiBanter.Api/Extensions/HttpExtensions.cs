using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Dto.Response;
using UAParser;

namespace DigiBanter.Api.Extensions
{
    public static class HttpExtensions
    {
        public static string GenerateDeviceIdentifier(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent);

            var ipAddress = httpContext.GetClientIpAddress() ?? "unknown";

            // Use UAParser to parse the User-Agent string
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);

            var browser = $"{clientInfo.UA.Family} {clientInfo.UA.Major}.{clientInfo.UA.Minor}";
            var platform = $"{clientInfo.OS.Family} {clientInfo.OS.Major}.{clientInfo.OS.Minor}";
            var device = clientInfo.Device.Family;

            var deviceInfo = $"{userAgent}|{ipAddress}|{browser}|{platform}|{device}";

            // Convert the device info to a Base64 string
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deviceInfo));
        }
        public static (string Browser, string OS, string DeviceType, string IpAddress, string UserAgent) GetDeviceInfo(this HttpContext context)
        {
            // Get User-Agent from headers
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);

            var browser = clientInfo.UA.Family + " " + clientInfo.UA.Major;
            var os = clientInfo.OS.ToString();
            var deviceType = clientInfo.Device.ToString();

            var ipAddress = context.GetClientIpAddress() ?? "unknown";

            // Return a tuple with the extracted data, including the user-agent string
            return (browser, os, deviceType, ipAddress, userAgent);
        }

        public static string? GetAccessToken(this string? authToken)
        {
            //var authToken = context.Request.Headers[HeaderNames.Authorization].ToString();

            if (string.IsNullOrEmpty(authToken))
                return authToken;

            var splitToken = authToken.Split(' ');

            if (splitToken.Length > 1)
                return splitToken[1];

            return authToken;
        }
        public static void SetTokensInsideCookie(this HttpContext context, TokenResponse token)
        {
            CookieOptions opt = new CookieOptions
            {
                IsEssential = true
            };
            context.Response.Cookies.Append("test", "pass", new CookieOptions
            {
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
            });
            context.Response.Cookies.Append(AppConstants.AccessToken, token.AccessToken, new CookieOptions
            {
                Expires = DateTimeOffset.FromUnixTimeSeconds(token.RefreshTokenExpires),
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
            });

            context.Response.Cookies.Append(AppConstants.RefreshToken, token.RefreshToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.FromUnixTimeSeconds(token.RefreshTokenExpires),
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/",
                });
        }

        public static string? GetClientIpAddress(this HttpContext httpContext)
        {
            return httpContext.Request.Headers.ContainsKey("X-Forwarded-For")
                ? httpContext.Request.Headers["X-Forwarded-For"].ToString()
                : httpContext.Connection.RemoteIpAddress?.ToString();
        }

    }
}
