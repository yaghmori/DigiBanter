
using System.Reflection;

namespace DigiBanter.Shared.Constatns;


public static class AppClaims
{
    public static class Application
    {
        public const string Dashboard = "dashboard";
        public const string Hangfire = "hangfire";
        public const string OpenAPi = "open-api";
        public const string AppSettings = "app-settings";
        public const string Chats = "chats";
        public const string Roles = "roles";
        public const string Users = "users";
        public const string Profiles = "profiles";
        public const string Post = "posts";
    }

    public static class Profiles
    {
        public const string AccessAll = Application.Profiles + ".access";
        public const string Edit = Application.Profiles + ".edit";
    }

    public static class Dashboards
    {
        public const string AccessAll = Application.Dashboard + ".access";
    }

    public static class Users
    {
        public const string AccessAll = Application.Users + ".access";

        public const string Sessions = Application.Users + ".sessions";
        public const string Sessions_Terminate = Application.Users + ".sessions.terminate";


        public const string Add = Application.Users + ".add";
        public const string Delete = Application.Users + ".delete";
        public const string Edit = Application.Users + ".edit";

        public const string Claims = Application.Users + ".claims";
        public const string Roles = Application.Users + ".roles";
        public const string Settings = Application.Users + ".settings";

    }

    public static class Roles
    {
        public const string AccessAll = Application.Roles + ".access";

        public const string Users = Application.Roles + ".users";
        public const string Claims = Application.Roles + ".claims";
        public const string Add = Application.Roles + ".add";
        public const string Edit = Application.Roles + ".edit";
        public const string Delete = Application.Roles + ".delete";
        public const string Filter = Application.Roles + ".filter";
    }
    public static class Post_Claim
    {
        public const string AccessAll = Application.Post + ".access";
        public const string Delete = Application.Post + ".delete";
        public const string Add = Application.Post + ".add";
        public const string Edit = Application.Post + ".edit";
    }


    /// <summary>
    /// Returns a list of Permissions.
    /// </summary>
    /// <returns></returns>
    public static List<string> GetPermissions()
    {
        var permissions = new List<string>();
        foreach (var prop in typeof(AppClaims).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
        {
            var propertyValue = prop.GetValue(null);
            var propertyValueString = propertyValue != null ? propertyValue.ToString() : string.Empty;
            if (!string.IsNullOrEmpty(propertyValueString))
                permissions.Add(propertyValueString);
        }
        return permissions;
    }
}

