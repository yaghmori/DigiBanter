using DigiBanter.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace DigiBanter.DataAccess
{
    public class AppDbContextContextContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql();

            // Pass null for IHttpContextAccessor, IPasswordHasher<MasterUser>, and ITenantResolver as they aren't needed at design-time
            return new AppDbContext(optionsBuilder.Options, null, null);
        }
    }

    public class AppDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor,
            IPasswordHasher<User> passwordHasher)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            // Provide a fallback password hasher for design-time
            _passwordHasher = passwordHasher ?? new PasswordHasher<User>();

        }

        #region DbSets
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostTranslation> PostTranslations { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagTranslation> TagTranslations { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }

        #endregion

    }
}
