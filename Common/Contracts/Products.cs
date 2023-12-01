namespace Common.Contracts;

public class Products
{
    // will be consume once product is created. will be called by RabbitMQ
    public record ProductCreated(Guid ProductId, string Name, decimal Price);
}
