namespace DigiBanter.Api.HostedServices;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DigiBanter.Domain.Entities;
using DigiBanter.DataAccess.DbContext;
using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Models;
using Microsoft.AspNetCore.Identity;
using static DigiBanter.Shared.Constatns.AppClaims;

public class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            
            // Apply migrations
            await dbContext.Database.MigrateAsync(cancellationToken);

            // Seed data (optional)
            await SeedDataAsync(scope.ServiceProvider, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during database initialization: {ex.Message}");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;


    public static async Task SeedDataAsync(IServiceProvider services, CancellationToken cancellationToken = default!)
    {
        var jsonSerializer = services.GetRequiredService<IJsonSerializer>();
        var appDbContext = services.GetRequiredService<AppDbContext>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var settings = configuration.Get<AppConfig>()!;
        var logger = services.GetRequiredService<ILogger<AppDbContext>>();

        // Start a new transaction
        using (var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                // Seed Users
                var userData = File.ReadAllText("SeedData/users.json");
                var users = jsonSerializer.Deserialize<List<User>>(userData);
                if (users != null && users.Any())
                {
                    foreach (var user in users)
                    {
                        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, settings.DefaultMigrationPassword);
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        user.NormalizedEmail = user.Email?.Normalize().ToUpper();
                        user.NormalizedUserName = user.UserName?.Normalize().ToUpper();

                        await appDbContext.Users.AddAsync(user, cancellationToken);
                    }
                }

                // Seed Roles
                var roleData = File.ReadAllText("SeedData/roles.json");
                var roles = jsonSerializer.Deserialize<List<Role>>(roleData);
                if (roles != null && roles.Any())
                {
                    foreach (var role in roles)
                    {
                        await appDbContext.Roles.AddAsync(role, cancellationToken);
                        if (role.Name == AppRoles.Administrator) // If it's admin then insert all claims
                        {
                            var list = new List<RoleClaim>();
                            foreach (var item in AppClaims.GetPermissions()) // Admin role claims
                            {
                                list.Add(new RoleClaim
                                {
                                    RoleId = role.Id,
                                    ClaimType = AppClaimTypes.ClaimType,
                                    ClaimValue = item,
                                });
                            }
                            await appDbContext.RoleClaims.AddRangeAsync(list);
                        }
                    }
                }



                // Seed UserRoles
                var userRolesData = File.ReadAllText("SeedData/user_roles.json");
                var userRoles = jsonSerializer.Deserialize<List<UserRole>>(userRolesData);
                if (userRoles != null && userRoles.Any())
                {
                    foreach (var userRole in userRoles)
                    {
                        await appDbContext.UserRoles.AddAsync(new UserRole
                        {
                            RoleId = userRole.RoleId,
                            UserId = userRole.UserId
                        }, cancellationToken);
                    }
                }

                                await appDbContext.SaveChangesAsync(cancellationToken);

                // Languages
                var languages = File.ReadAllText("SeedData/languages.json");
                var languages_data = jsonSerializer.Deserialize<List<Language>>(languages);
                if (languages_data != null && languages_data.Any())
                {
                    await appDbContext.AddRangeAsync(languages_data, cancellationToken);
                }

                // category
                var categories = File.ReadAllText("SeedData/categories.json");
                var category_data = jsonSerializer.Deserialize<List<Category>>(categories);
                if (category_data != null && category_data.Any())
                {
                    await appDbContext.AddRangeAsync(category_data, cancellationToken);
                }


                // category_translation
                var category_translations = File.ReadAllText("SeedData/category_translations.json");
                var category_translations_data = jsonSerializer.Deserialize<List<CategoryTranslation>>(category_translations);
                if (category_translations_data != null && category_translations_data.Any())
                {
                    await appDbContext.AddRangeAsync(category_translations_data, cancellationToken);
                }

                // post
                var posts = File.ReadAllText("SeedData/posts.json");
                var posts_data = jsonSerializer.Deserialize<List<Post>>(posts);
                if (posts_data != null && posts_data.Any())
                {
                    await appDbContext.AddRangeAsync(posts_data, cancellationToken);
                }


                // post_translation
                var post_translation = File.ReadAllText("SeedData/post_translations.json");
                var post_translation_data = jsonSerializer.Deserialize<List<PostTranslation>>(post_translation);
                if (post_translation_data != null && post_translation_data.Any())
                {
                    await appDbContext.AddRangeAsync(post_translation_data, cancellationToken);
                }


                // Save all changes within the transaction
                await appDbContext.SaveChangesAsync(cancellationToken);

                // Commit the transaction
                await transaction.CommitAsync(cancellationToken);

                logger.LogInformation("Successfully seeded all master data.");

            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync(cancellationToken);

                logger.LogError(ex, "An error occurred while seeding master data.");

            }
        }
    }


}
