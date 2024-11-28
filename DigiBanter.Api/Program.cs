using AutoMapper;
using DigiBanter.Api.ActionFilters;
using DigiBanter.Api.Middlewares;
using DigiBanter.DataAccess;
using DigiBanter.Domain.Entities;
using DigiBanter.Shared;
using DigiBanter.Shared.Models;
using DigiBanter.Shared.ResultWrapper;
using DigiBanter.Shared.Services.TimeZoneResolver;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml;


AppConfig _config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
_config = builder.Configuration.Get<AppConfig>()!;


BindAppConfiguration(builder);
ConfigureControllerJsonSerializer(builder);
ConfigureValidators(builder);
ConfigureDbContexts(builder);
ConfigureIdentity(builder);
ConfigureMapper(builder);
builder.Services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<TimeZoneMiddleware>();



List<PodcastItem> podcastList = [  new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/11/b-02-1-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2021/12/b-01-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 07: Goodbye boring, hello adventure"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/b-03-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 04: The Digital Nomad Lifestyle Facts"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/b-04-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 03: Dating As a Digital Nomad"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/b-06-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/11/b-02-1-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/11/ahmetoz_van_gogh_style_adba5b8a-7c99-4218-972c-911812166326-550x550.png", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/amanda-souza-PPTiicBrVU4-unsplash-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/b-06-300x300.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 07: Goodbye boring, hello adventure"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/nathan-dumlao-g-De0Zd2hZ0-unsplash-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 04: The Digital Nomad Lifestyle Facts"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/louis-hansel-shotsoflouis-GW0xfjw3IMg-unsplash-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 03: Dating As a Digital Nomad"),
    new PodcastItem(Guid.NewGuid(), DateTime.Now, "https://themes.pixelwars.org/podcasty/demo-01/wp-content/uploads/sites/2/2020/09/dan-gold-xlNqmgDDYp0-unsplash-550x550.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    ];

app.Map("/", () => "hello world!");
app.Map("/podcast", async () =>
{
    await Task.Delay(2000);

    return Results.Ok(ApiResult<IList<PodcastItem>>.Success(podcastList));
});

app.Map("/podcast/{id}", async (string id) =>
{
    await Task.Delay(1000);

    var podcast = podcastList.FirstOrDefault(x => x.Id.ToString() == id);
    if (podcast == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(ApiResult<PodcastItem>.Success(podcast));
});
app.Run();
void BindAppConfiguration(WebApplicationBuilder builder)
{

    var env = builder.Environment.EnvironmentName;
    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", true, true)
        .AddEnvironmentVariables();

    builder.Services.AddOptions<AppConfig>()
    .BindConfiguration(nameof(AppConfig));


    _config = builder.Configuration.Get<AppConfig>()!;

    if (_config == null)
    {
        throw new Exception("Application settings not found");
    }


}
void ConfigureControllerJsonSerializer(WebApplicationBuilder builder)
{

    var jsonSettings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Newtonsoft.Json.Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,

        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        DateParseHandling = DateParseHandling.DateTimeOffset,
        //Converters = { new IsoDateTimeConverter { DateTimeFormat = "o" } }

    };



    builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = jsonSettings.ReferenceLoopHandling;
        options.SerializerSettings.Formatting = jsonSettings.Formatting;
        options.SerializerSettings.NullValueHandling = jsonSettings.NullValueHandling;
        options.SerializerSettings.DateTimeZoneHandling = jsonSettings.DateTimeZoneHandling;
        options.SerializerSettings.DateFormatHandling = jsonSettings.DateFormatHandling;
        options.SerializerSettings.DateParseHandling = jsonSettings.DateParseHandling;
        options.SerializerSettings.Converters = jsonSettings.Converters;

    });

    builder.Services.AddSingleton(jsonSettings);
    builder.Services.AddScoped<ModelValidationAttribute>();

    builder.Services.AddMvc(config => config.RespectBrowserAcceptHeader = true);

    builder.Services.AddSingleton<IJsonSerializer>(sp =>
    {
        return new NewtonsoftJsonSerializer(jsonSettings);

        // return new SystemTextJsonSerializer(serializerOptions);
    });
}
void ConfigureValidators(WebApplicationBuilder builder)
{
    builder.Services.AddValidatorsFromAssemblyContaining(typeof(DigiBanter.Shared.Models.AppConfig));
}

void ConfigureDbContexts(WebApplicationBuilder builder)
{
    var environment = builder.Environment;

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(_config.DatabaseOptions.ConnectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
        });


        // Enable sensitive data logging in development only
        if (environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
    });

}

void ConfigureIdentity(WebApplicationBuilder builder)
{
    builder.Services.AddIdentity<User, DigiBanter.Domain.Entities.Role>(options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredLength = 1;
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.AllowedForNewUsers = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    builder.Services.AddScoped<IPasswordHasher<AppDbContext>, PasswordHasher<AppDbContext>>();
}
void ConfigureMapper(WebApplicationBuilder builder)
{
    builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new MappingProfile(provider.GetService<ITimeZoneProvider>()!, provider.GetService<IJsonSerializer>()!));
    }).CreateMapper());
}