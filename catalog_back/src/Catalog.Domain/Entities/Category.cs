namespace Catalog.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    public static Category Create(string name, string description)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public Category()
    {
    }
}
