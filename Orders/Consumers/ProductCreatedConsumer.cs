using System.Threading.Tasks;
using Common.Repositories;
using MassTransit;
using Orders.Entities;
using static Common.Contracts.Products;

namespace Orders.Consumers
{

    // The function will be called RabbitMQ. every class implement `IConsumer<ProductCreated>` will be called. ProductCreated is from Common contracts Products.cs 
    // from proj Products we are publishing to RabbitMQ - publishEndpoint.Publish(new ProductCreated(product.Id, ...)

    public class ProductCreatedConsumera : IConsumer<ProductCreated> //ProductCreated: common cotracts
    {
        private readonly IRepository<Product> productRepository;

        public ProductCreatedConsumera(IRepository<Product> repository)
        {
            productRepository = repository;
        }

        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var message = context.Message;

            var item = await productRepository.GetAsync(message.ProductId);

            if (item != null)
            {
                return;
            }

            item = new Product
            {
                Id = message.ProductId,
                Name = message.Name,
                Price = message.Price,
                DateCreated = DateTime.UtcNow,

            };
            await productRepository.CreateAsync(item);
        }
    }
}