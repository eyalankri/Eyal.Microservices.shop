using Common;

namespace Orders.Entities
{
    public class Order : IEntity
    {
        public Order()
        {
            DateCreated = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Guid>? ProductIds { get; set; }
        public Guid UserId { get; set; }

    }
}
