using Microsoft.AspNetCore.WebUtilities;
using System.Reflection;

namespace ParsLinks.Shared.Extensions;


public static class QueryParameterHelper
{
    public static string ToQueryString<T>(this T parameters) where T : class
    {
        var queryParams = new Dictionary<string, string>();

        if (parameters == null)
            return string.Empty;

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var value = prop.GetValue(parameters);
            if (value != null)
            {
                queryParams[prop.Name] = value.ToString();
            }
        }

        return QueryHelpers.AddQueryString(string.Empty, queryParams);
    }
}
