using Common;

namespace Orders.Entities;

public class Product : IEntity
{
    //public Product()
    //{
    //    DateCreated = DateTime.UtcNow;

    //}

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}
