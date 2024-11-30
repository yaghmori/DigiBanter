using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

public static class RouteInspector
{
    public static void FindDuplicateRoutes()
    {
        var controllers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);

        foreach (var controller in controllers)
        {
            var routes = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(method => method.GetCustomAttributes().OfType<HttpMethodAttribute>().Any())
                .SelectMany(method =>
                    method.GetCustomAttributes()
                        .OfType<HttpMethodAttribute>()
                        .Select(attr => new
                        {
                            Controller = controller.Name,
                            Action = method.Name,
                            Route = attr.Template ?? string.Empty,
                            HttpMethod = attr.HttpMethods.FirstOrDefault() ?? "Any"
                        }))
                .GroupBy(route => new { route.Route, route.HttpMethod })
                .Where(group => group.Count() > 1)
                .ToList();

            if (routes.Any())
            {
                //Log.Error($"Duplicate routes found in controller: {controller.Name}");
                foreach (var routeGroup in routes)
                {
                    //Log.Error($"  Route: '{routeGroup.Key.Route}' Method: '{routeGroup.Key.HttpMethod}'");
                    foreach (var route in routeGroup)
                    {
                        //Log.Error($"    Action: {route.Action}");
                    }
                }
            }
        }
    }
}
