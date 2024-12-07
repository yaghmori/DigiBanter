using ParsLinks.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace ParsLinks.DataAccess.EntityTypeConfiguration;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("UserSessions");

        builder.HasKey(us => us.Id);

        #region Navigation Properties
        builder.HasOne(us => us.User)
            .WithMany()
            .HasForeignKey(us => us.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
