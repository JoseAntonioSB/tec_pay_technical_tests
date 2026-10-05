using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Catalog.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.HasIndex(c => c.Name).IsUnique();
        builder.HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict); // added in order to avoid issues when deleting a category that has products associated with it
        
    }
}