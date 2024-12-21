using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text.Json;

namespace ParsLinks.Shared.Extensions
{
    public static class JsonPatchDocumentExtensions
    {
        public static void AddReplaceIfChanged<TRequest, T>(
            this JsonPatchDocument<TRequest> patchDoc,
            Expression<Func<TRequest, T>> path,
            T originalValue,
            T newValue)
            where TRequest : class
        {
            if (!EqualityComparer<T>.Default.Equals(originalValue, newValue))
            {
                patchDoc.Replace(path, newValue);
            }
        }

        public static bool RemoveOperation<T>(this JsonPatchDocument<T> patchDoc, string path) where T : class
        {
            if (patchDoc is null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            // Find the operation to remove
            var operationToRemove = patchDoc.Operations.FirstOrDefault(op => op.path.Equals(path, StringComparison.OrdinalIgnoreCase));

            // Remove the operation if it exists
            if (operationToRemove != null)
            {
                patchDoc.Operations.Remove(operationToRemove);
                return true;
            }
            return false;
        }


    }


    public static class JsonOptions
    {
        public static JsonSerializerOptions systemJsonSettings => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,

            //Converters = { new CustomDateTimeFormatConverter("MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture) }
        };
        public static JsonSerializerSettings newtonJsonSettings => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "MM-dd-yyyy HH:mm:ss",
        };

    }


}
