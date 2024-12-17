using Microsoft.Extensions.FileProviders;
using ParsLinks.Api.Endpoints;
using ParsLinks.Api.Extensions;
using ParsLinks.Api.Middlewares;
using ParsLinks.Api.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.BindConfigurations();
builder.RegisterServices();

builder.ConfigureAuthentication();
builder.ConfigureAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();
var app = builder.Build();

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseMiddlewares();

app.MapOpenApi();

app.UseAntiforgery();

//app.MapScalarApiReference(options =>
//{
//    options.WithTitle("Welcome to parslink api");
//        // Fluent API
//        //options
//        //    .WithPreferredScheme("ApiKey") // Security scheme name from the OpenAPI document
//        options.WithApiKeyAuthentication(apiKey =>
//        {
//            apiKey.Token = "your-api-key";
//        });
//});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider("/app/assets"),
    RequestPath = "/assets" // This is the URL that the files will be accessible from
});

app.MapEndpoints();

app.Run();