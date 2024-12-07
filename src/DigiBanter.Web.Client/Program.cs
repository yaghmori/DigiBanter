using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DigiBanter.Web.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.RegisterServices(builder.HostEnvironment.Environment);

await builder.Build().RunAsync();


