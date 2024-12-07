using ParsLinks.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace ParsLinks.DataAccess.EntityTypeConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        #region Navigation Properties
        builder.HasMany(u => u.UserClaims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId);

        builder.HasMany(u => u.UserLogins)
            .WithOne()
            .HasForeignKey(ul => ul.UserId);

        builder.HasMany(u => u.UserTokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId);

        builder.HasMany(u => u.UserSessions)
            .WithOne()
            .HasForeignKey(us => us.UserId);

        builder.HasMany(u => u.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId);

        builder.HasMany(u => u.Posts)
            .WithOne()
            .HasForeignKey(ur => ur.AuthorId);

        builder.HasMany(u => u.Comments)
    .WithOne()
    .HasForeignKey(ur => ur.UserId);

        #endregion

        builder.Property(x => x.PublicId)
               .ValueGeneratedOnAdd()
               .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
