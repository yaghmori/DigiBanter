using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DigiBanter.Web.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.RegisterServices();

await builder.Build().RunAsync();


