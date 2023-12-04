using Orders.Entities;

namespace Orders
{
    public static class Extensions
    {
        public static ProductDto AsDto(this Product p) {
         
            return new ProductDto(p.Id, p.Name != null ? p.Name : "", p.Description != null ? p.Description : "", p.Price, p.CategoryId, p.DateCreated);
        }
    }
}
