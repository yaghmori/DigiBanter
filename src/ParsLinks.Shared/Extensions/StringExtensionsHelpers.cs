using System.Text.RegularExpressions;

namespace ParsLinks.Shared.Extensions;

public static class StringExtensionsHelpers
{

    public static string GenerateSlug(string title)
    {
        if (string.IsNullOrEmpty(title))
            return string.Empty;

        // Convert to lowercase
        string slug = title.ToLowerInvariant();

        // Remove all special characters and replace spaces with hyphens
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

        // Replace multiple spaces with a single space
        slug = Regex.Replace(slug, @"\s+", " ").Trim();

        // Replace spaces with hyphens
        slug = slug.Replace(" ", "-");

        // Ensure slug is not too long
        if (slug.Length > 100)
            slug = slug.Substring(0, 100);

        return slug;
    }
}