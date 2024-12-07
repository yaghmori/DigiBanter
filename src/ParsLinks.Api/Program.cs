using ParsLinks.Api.Endpoints;
using ParsLinks.Api.Extensions;
using ParsLinks.Api.Middlewares;
using ParsLinks.Api.Services;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.BindConfigurations();
builder.RegisterServices();

builder.ConfigureAuthentication();
builder.ConfigureAuthorization();

builder.Services.AddOpenApi();
var app = builder.Build();

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseMiddlewares();

app.MapOpenApi();


app.MapScalarApiReference(options =>
{
    options.WithTitle("Welcome to digibante.api");
        // Fluent API
        //options
        //    .WithPreferredScheme("ApiKey") // Security scheme name from the OpenAPI document
        options.WithApiKeyAuthentication(apiKey =>
        {
            apiKey.Token = "your-api-key";
        });

    //// Object initializer
    //options.Authentication = new ScalarAuthenticationOptions
    //{
    //    PreferredSecurityScheme = "ApiKey", // Security scheme name from the OpenAPI document
    //    ApiKey = new ApiKeyOptions
    //    {
    //        Token = "your-api-key"
    //    }
    //};
    //options
    //.WithPreferredScheme("OAuth2") // Security scheme name from the OpenAPI document
    //.WithOAuth2Authentication(oauth =>
    //{
    //    oauth.ClientId = "public-client";
    //    oauth.Scopes = ["profile"];
    //});
    //// Basic
    //options
    //    .WithPreferredScheme("Basic") // Security scheme name from the OpenAPI document
    //    .WithHttpBasicAuthentication(basic =>
    //    {
    //        basic.Username = "your-username";
    //        basic.Password = "your-password";
    //    });

    // Bearer
    //options
    //    .WithPreferredScheme("Bearer") // Security scheme name from the OpenAPI document
    //    .WithHttpBearerAuthentication(bearer =>
    //    {
    //        bearer.Token = "your-bearer-token";
    //    });


});

app.MapEndpoints();

app.Run();