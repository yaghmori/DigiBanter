using ParsLinks.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace ParsLinks.DataAccess.EntityTypeConfiguration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(p => p.Author)
               .WithMany()
               .HasForeignKey(p => p.AuthorId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Posts)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.Translations)
.WithOne()
.HasForeignKey(ur => ur.PostId);

        builder.HasMany(u => u.Comments)
.WithOne()
.HasForeignKey(ur => ur.PostId);

        builder.HasMany(p => p.Tags)
       .WithMany(t => t.Posts) // Define inverse skip navigation
       .UsingEntity<PostTag>(
           j => j.HasOne(pt => pt.Tag)
                 .WithMany()
                 .HasForeignKey(pt => pt.TagId)
                 .OnDelete(DeleteBehavior.Cascade),
           j => j.HasOne(pt => pt.Post)
                 .WithMany()
                 .HasForeignKey(pt => pt.PostId)
                 .OnDelete(DeleteBehavior.Cascade),
           j => j.HasKey(pt => new { pt.PostId, pt.TagId }) // Composite key
       );

    }
}
