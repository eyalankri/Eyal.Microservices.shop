using Orders.Entities;

namespace Orders
{
    public static class Extensions
    {
        public static OrderDto AsDto(this Order order, List<Product> products)
        {
            // change guid to the userId
            
            return new OrderDto(order.Id, Guid.NewGuid(), order.DateCreated, order.TotalPrice, products);
        }
    }
}
