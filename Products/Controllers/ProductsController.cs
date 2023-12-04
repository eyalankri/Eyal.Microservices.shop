using Common.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Entities;
using static Common.Contracts.Products;

namespace Orders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> productRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public ProductsController(IRepository<Product> productRepository, IPublishEndpoint publishEndpoint)
        {
            this.productRepository = productRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ProductDto>> PostAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                CategoryId = createProductDto.CategoryId,
                DateCreated = DateTime.UtcNow
            };

            await productRepository.CreateAsync(product);
            try
            {
                // publish product to orders-products
                await publishEndpoint.Publish(new ProductCreated(product.Id, product.Name, product.Price));
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> GetAllAsync()
        { 
        
            var items = (await productRepository.GetAllAsync()).Select(p=> p.AsDto());
            return Ok(items);
        }
    }
}
