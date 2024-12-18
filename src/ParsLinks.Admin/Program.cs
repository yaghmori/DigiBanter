using ParsLinks.Admin.Components;
using ParsLinks.Admin.Extensions;
using Radzen;
using Sitko.Blazor.CKEditor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.RegisterServices(builder.Environment.EnvironmentName);

builder.Services.AddRadzenComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddCKEditor(builder.Configuration, options =>
{
    options.ScriptPath = "https://cdn.ckeditor.com/ckeditor5/28.0.0/classic/ckeditor.js";
    options.EditorClassName = "ClassicEditor";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
