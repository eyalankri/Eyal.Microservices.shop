using System.Threading.Tasks;
using Common.Repositories;
using MassTransit;
using Orders.Entities;
using static Common.Contracts.Products;

namespace Orders.Consumers
{
    public class ProductCreatedTestConsumer : IConsumer<ProductCreated> //ProductCreated: common cotracts
    {
        private readonly IRepository<Product> productRepository;

        public ProductCreatedTestConsumer(IRepository<Product> repository)
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