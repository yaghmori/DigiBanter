using DigiBanter.Application.Extensions;
using DigiBanter.Shared.Models;
using DigiBanter.Shared.Services.TimeZoneResolver;
using DigiBanter.Shared.Services;
using DigiBanter.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AutoMapper;
using DigiBanter.Shared.Constatns;
using DigiBanter.Web.Client.HttpDelegateHnadlers;
using DigiBanter.Shared;
using FluentValidation;
using Newtonsoft.Json;
using DigiBanter.Web.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.RegisterServices();

await builder.Build().RunAsync();


