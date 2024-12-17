using ParsLinks.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace ParsLinks.DataAccess.EntityTypeConfiguration;

public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
{
    public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();



        builder.HasOne(ct => ct.Category)
               .WithMany(c => c.Translations)
               .HasForeignKey(ct => ct.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ct => ct.Language)
               .WithMany(l => l.CategoryTranslations)
               .HasForeignKey(ct => ct.LanguageId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
