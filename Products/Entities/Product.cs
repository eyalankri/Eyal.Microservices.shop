using Common;

namespace Orders.Entities
{
    public class Product : IEntity
    {
        public Product()
        {
            DateCreated = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public DateTimeOffset DateCreated { get; set; }

    }
}
