using ParsLinks.Shared.Constatns;
using System.Security.Claims;
using System.Text.Json;

namespace ParsLinks.Shared.Helpers
{
    public static class JwtParser
    {

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


    }
}

