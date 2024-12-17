using ParsLinks.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace ParsLinks.DataAccess.EntityTypeConfiguration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.HasMany(t => t.Translations)
               .WithOne(tt => tt.Tag)
               .HasForeignKey(tt => tt.TagId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
