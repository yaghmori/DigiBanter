using ParsLinks.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace ParsLinks.DataAccess.EntityTypeConfiguration;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable("UserLogins");

        builder.HasKey(ul => ul.Id);

        #region Navigation Properties
        builder.HasOne(ul => ul.User)
            .WithMany(u => u.UserLogins)
            .HasForeignKey(ul => ul.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
