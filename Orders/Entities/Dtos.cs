using System.ComponentModel.DataAnnotations;
namespace Orders.Entities;

public record OrderDto(Guid Id, Guid UserId, DateTimeOffset DateCreated, decimal TotalPrice, List<Product>? Products);
public record CreateOrdertDto([Required] Guid UserId, [Required] List<Guid> ProductIds);
public record ProductDto(Guid Id, string Name, decimal Price);


