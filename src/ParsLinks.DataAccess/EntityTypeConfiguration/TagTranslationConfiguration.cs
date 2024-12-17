using ParsLinks.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace ParsLinks.DataAccess.EntityTypeConfiguration;
public class TagTranslationConfiguration : IEntityTypeConfiguration<TagTranslation>
{
    public void Configure(EntityTypeBuilder<TagTranslation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Slug)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasOne(t => t.Tag)
               .WithMany(t => t.Translations)
               .HasForeignKey(t => t.TagId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Language)
               .WithMany(l => l.TagTranslations)
               .HasForeignKey(t => t.LanguageId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
