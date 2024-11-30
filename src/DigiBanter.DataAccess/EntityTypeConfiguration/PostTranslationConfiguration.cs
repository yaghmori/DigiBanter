using DigiBanter.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DigiBanter.DataAccess.EntityTypeConfiguration;

public class PostTranslationConfiguration : IEntityTypeConfiguration<PostTranslation>
{
    public void Configure(EntityTypeBuilder<PostTranslation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(pt => pt.Post)
               .WithMany(p => p.Translations)
               .HasForeignKey(pt => pt.PostId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pt => pt.Language)
               .WithMany(l => l.PostTranslations)
               .HasForeignKey(pt => pt.LanguageId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
