using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsLinks.Shared.Constatns;
public static class AppEndPoints
{




    public const string Index = "/";

    public const string BaseAddress = "";
    public const string MainHub = "/hubs/main";
    public const string IdentityHub = "/hubs/identity";
    public const string ChatHub = "/hubs/chat";
    public const string Hangfire = "/hangfire";
    public const string HealthCheck = "/healthz";
    public const string Swagger = "/swagger";
    public const string Scalar = "/scalar/v1";

    public static class Podcast
    {
        public const string Base = BaseAddress + "/podcast";
        public const string GetById = Base + "/{0}";
    }
    public static class BlogPosts
    {
        public const string Base = BaseAddress + "/posts";
        public const string GetById = Base + "/{0}";
    }
    public static class Seed
    {
        public const string Base = BaseAddress + "/seed";
    }

    public static class Auth
    {
        public const string Base = BaseAddress + "/auth";
        public const string Login = Base + "/login";
        public const string Register = Base + "/register";
    }

    public static class User
    {
        public const string Base = BaseAddress + "/users";
        public const string GetUsers = Base ;
        public const string GetUserById = Base + "/{0}";
    }

}