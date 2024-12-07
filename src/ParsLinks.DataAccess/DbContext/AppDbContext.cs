using ParsLinks.DataAccess.EntityTypeConfiguration;
using ParsLinks.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection.Emit;

namespace ParsLinks.DataAccess.DbContext;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{

    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql();

        // Pass null for IHttpContextAccessor, IPasswordHasher<MasterUser>, and ITenantResolver as they aren't needed at design-time
        return new AppDbContext(optionsBuilder.Options);
    }

}



public class AppDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql();

    }

    #region DbSets
    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<PostTag> PostTags { get; set; }
    public virtual DbSet<PostTranslation> PostTranslations { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<TagTranslation> TagTranslations { get; set; }
    public virtual DbSet<UserSession> UserSessions { get; set; }

    #endregion


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Ensure Identity configurations are applied first
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }


}
