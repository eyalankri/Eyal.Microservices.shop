using Orders.Entities;

namespace Orders
{
    public static class Extensions
    {
        public static OrderDto AsDto(this Order order, List<Product> products)
        {
            
            return new OrderDto(order.Id, order.DateCreated, order.TotalPrice, products);
        }
    }
}
