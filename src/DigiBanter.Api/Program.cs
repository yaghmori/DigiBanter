using DigiBanter.Api.Endpoints;
using DigiBanter.Api.Extensions;
using DigiBanter.Api.Middlewares;
using DigiBanter.Api.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.BindConfigurations();
builder.RegisterServices();
var app = builder.Build();

//app.UseHttpsRedirection();
app.UseMiddlewares();
app.MapEndpoints();
app.Run();