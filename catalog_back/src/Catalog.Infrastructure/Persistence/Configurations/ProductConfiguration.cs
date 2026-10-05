using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.HasIndex(p => p.Name);
        
    }
}