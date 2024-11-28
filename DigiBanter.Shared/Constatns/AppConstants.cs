

namespace DigiBanter.Shared.Constatns;
public static class AppConstants
{
    public static string BaseUrl = string.Empty;
    public static string ServerBaseAddress = "";
    public static string AvatarPreview = "/assets/images/avatar.jpg";
    public static string NoPreview = "/assets/images/nopreview.jpg";
    public static string Logo = "/assets/logo/logo.png";
    public static string clientBaseAddress = "";
    public const string TenantId = "tid";
    public static string Agent = "agent";
    public static string UserAgent = "User-Agent";
    public static string TimeZoneId = "TimeZoneId";
    public static string IpAddressHeader = "X-Forwarded-For";
    public static string Culture = "culture";
    public static string IsPersistent = "ispersistent";
    public static string Preference = "clientPreference";
    public static string AccessToken = "access_token";
    public static string RefreshToken = "refresh_token";
    public static string UserImageURL = "userImageURL";
    public static string DefaultConnectionString = "DefaultConnectionString";
    public static string DefaultAppSettingsKey = "DefaultAppSettings";
    public static string BackupAppSettingsKey = "BackupAppSettings";
    public static string SessionState = "session_state";
    public static string Session = "session";

}
public static class AppCacheKeys
{
    public static string OnlineUsersKey = "online_users_tenant_";
    public static string TenantKey = "tenants_";
}

public static class AppClientTypes
{
    public static string Identity = "client.identity";
    public static string Master = "client.master";
}
