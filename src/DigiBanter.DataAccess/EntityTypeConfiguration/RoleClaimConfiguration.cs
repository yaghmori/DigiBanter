using DigiBanter.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DigiBanter.DataAccess.EntityTypeConfiguration;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");

        builder.HasKey(rc => rc.Id);

        #region Configure Relationship
        builder.HasOne(rc => rc.Role)
            .WithMany(r => r.RoleClaims)
            .HasForeignKey(rc => rc.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
