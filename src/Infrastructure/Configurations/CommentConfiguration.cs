using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.Id)
            .HasName("PK_Comments");

        builder.Property(c => c.Description)
            .IsRequired()
            .HasColumnType("NVARCHAR(250)");

        builder.Property(c => c.CreatedOn)
               .IsRequired()
               .HasColumnType("DATETIME")
               .HasDefaultValueSql("GETDATE()");

        builder.Property(c => c.UpdatedOn)
               .IsRequired(false)
               .HasColumnType("DATETIME");

        builder.HasOne(c => c.Blog)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.BlogId)
            .HasConstraintName("FK_Comment_Blog")
            .IsRequired();
    }
}
