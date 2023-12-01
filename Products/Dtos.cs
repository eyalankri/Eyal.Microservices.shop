using System;
using System.ComponentModel.DataAnnotations;

namespace Products
{
    // record: a simple immutible type that is not changed after it is created
    public record ProductDto(Guid Id, string Name, string Description, decimal Price, int CategoryId, DateTimeOffset DateCreated);

    public record CreateProductDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price, int CategoryId);

    public record UpdateProductDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price, int CategoryId);
}


