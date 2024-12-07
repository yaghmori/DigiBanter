namespace ParsLinks.Shared.Constatns;

public static class AppUri
{
    public static string Logout = $"{AppConstants.AuthBaseAddress}";
    public static string Register = $"{AppConstants.AuthBaseAddress}";
    public static string login = $"{AppConstants.AuthBaseAddress}";

    public const string Index = "/";
    public const string Dashboard = "/";
    public const string Users = "/users";
    public const string Posts = "/posts";
    public const string post = "/post";

    public static string Hangfire = $"{AppConstants.ApiBaseAddress}{AppEndPoints.Hangfire}";
    public static string Api = $"{AppConstants.ApiBaseAddress}{AppEndPoints.Scalar}";
    public static string HealthCheck = $"{AppConstants.ApiBaseAddress}{AppEndPoints.HealthCheck}";



    /// <summary>
    /// Returns a list of Permissions.
    /// </summary>
    /// <returns></returns>
    public static List<string> GetUrls()
    {
        var permissions = new List<string>();
        foreach (var prop in typeof(AppUri).GetFields())
        {
            var propertyValue = prop.GetValue(null)?.ToString();
            if (!string.IsNullOrEmpty(propertyValue))
                permissions.Add(propertyValue);
        }
        return permissions;
    }

}


