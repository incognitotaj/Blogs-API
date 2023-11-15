using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("Blogs");

        builder.HasKey(b => b.Id)
            .HasName("PK_Blogs");

        builder.Property(b => b.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR(250)");

        builder.Property(b => b.Description)
            .IsRequired()
            .HasColumnType("NVARCHAR(500)");

        builder.Property(b => b.CreatedOn)
               .IsRequired()
               .HasColumnType("DATETIME")
               .HasDefaultValueSql("GETDATE()");

        builder.Property(b => b.UpdatedOn)
               .IsRequired(false)
               .HasColumnType("DATETIME");
    }
}
