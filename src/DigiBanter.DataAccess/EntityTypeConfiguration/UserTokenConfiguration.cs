using DigiBanter.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DigiBanter.DataAccess.EntityTypeConfiguration;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens");

        builder.HasKey(ut => ut.Id);

        #region Navigation Properties
        builder.HasOne(ut => ut.User)
            .WithMany()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
