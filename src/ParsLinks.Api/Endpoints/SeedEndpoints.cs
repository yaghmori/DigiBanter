using ParsLinks.DataAccess.DbContext;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.ResultWrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

public static class SeedEndpoints
{
    public static void MapSeedEndpoints(this IEndpointRouteBuilder routes)
    {
        var post = routes.MapGroup(AppEndPoints.Seed.Base);


        post.MapPost("/seed", SeedData)
            .AllowAnonymous();
        
        post.MapPost("/migrate", MigrateDatabase)
            .AllowAnonymous();
    }




    private static async Task<IResult> MigrateDatabase(HttpContext context,
        AppDbContext appDbContext,
        CancellationToken cancellationToken)
    {
        await appDbContext.Database.MigrateAsync(cancellationToken);
        return TypedResults.Ok(ApiResult.Success("Migration Successfull"));
    }
    private static async Task<IResult> SeedData(HttpContext context,
        IServiceProvider services,
        CancellationToken cancellationToken)
    {
        await SeedDataAsync(services);
        return TypedResults.Ok(ApiResult.Success("Successfull"));
    }
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
                var userData = File.ReadAllText("data/seed/users.json");
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
                var roleData = File.ReadAllText("data/seed/roles.json");
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
                                    ClaimType = AppClaimTypes.auth,
                                    ClaimValue = item,
                                });
                            }
                            await appDbContext.RoleClaims.AddRangeAsync(list);
                        }
                    }
                }



                // Seed UserRoles
                var userRolesData = File.ReadAllText("data/seed/user_roles.json");
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
                var languages = File.ReadAllText("data/seed/languages.json");
                var languages_data = jsonSerializer.Deserialize<List<Language>>(languages);
                if (languages_data != null && languages_data.Any())
                {
                    await appDbContext.AddRangeAsync(languages_data, cancellationToken);
                }

                // category
                var categories = File.ReadAllText("data/seed/categories.json");
                var category_data = jsonSerializer.Deserialize<List<Category>>(categories);
                if (category_data != null && category_data.Any())
                {
                    await appDbContext.AddRangeAsync(category_data, cancellationToken);
                }


                // category_translation
                var category_translations = File.ReadAllText("data/seed/category_translations.json");
                var category_translations_data = jsonSerializer.Deserialize<List<CategoryTranslation>>(category_translations);
                if (category_translations_data != null && category_translations_data.Any())
                {
                    await appDbContext.AddRangeAsync(category_translations_data, cancellationToken);
                }

                // post
                var posts = File.ReadAllText("data/seed/posts.json");
                var posts_data = jsonSerializer.Deserialize<List<Post>>(posts);
                if (posts_data != null && posts_data.Any())
                {
                    await appDbContext.AddRangeAsync(posts_data, cancellationToken);
                }


                // post_translation
                var post_translation = File.ReadAllText("data/seed/post_translations.json");
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
