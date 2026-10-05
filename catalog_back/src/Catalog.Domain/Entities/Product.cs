namespace Catalog.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Update(string name, string description, decimal price, bool isActive, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        IsActive = isActive;
        CategoryId = categoryId;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Product Create(string name, string? description, decimal price, int stock, Guid categoryId)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
            IsActive = true,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
