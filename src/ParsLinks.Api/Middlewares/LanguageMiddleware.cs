using ParsLinks.Api.Services;

namespace ParsLinks.Api.Middlewares;

public class LanguageMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    private static readonly Dictionary<string, string> LanguageMap = new()
    {
        { "en", "en-US" },
        { "fr", "fr-FR" },
        { "fa", "fa-IR" }
    };
    public async Task Invoke(HttpContext context,ILanguageService languageService)
    {
        // Try to get `lang` from route or query string
        var lang = context.Request.RouteValues["lang"]?.ToString() ??
                   context.Request.Query["lang"].FirstOrDefault();

        // If no lang is provided, default to "en-US"
        if (string.IsNullOrEmpty(lang))
        {
            lang = "en-US";
        }
        else if (LanguageMap.TryGetValue(lang, out var fullLang))
        {
            // Map shorthand to full culture name
            lang = fullLang;
        }
        else
        {
            // If the provided `lang` isn't recognized, you can:
            // Option 1: Use a default language
            lang = "en-US";

            // Option 2: Reject the request with an error
            // return Results.BadRequest("Unsupported language");
        }


        // Store the extracted language in HttpContext.Items for global access
        context.Items["lang"] = lang;
        languageService.Language = lang;
        // Optional: Set the culture for the current thread
        var culture = new System.Globalization.CultureInfo(lang);
        System.Globalization.CultureInfo.CurrentCulture = culture;
        System.Globalization.CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}