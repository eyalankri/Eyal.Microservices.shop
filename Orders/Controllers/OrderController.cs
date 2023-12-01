using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using Orders.Entities;

namespace Orders.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IRepository<Order> orderRepository;
    private readonly IRepository<Product> productRepository;

    public OrderController(IRepository<Order> orderRepository, IRepository<Product> productRepository)
    {
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(CreateOrdertDto createOrdertDto)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            ProductIds = createOrdertDto.ProductIds,     
            UserId = createOrdertDto.UserId
        };

        await orderRepository.CreateAsync(order);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<OrderDto>> GetAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            return BadRequest();
        }
        var order = await orderRepository.GetAsync(orderId);
        if (order.ProductIds?.Count == 0 )
        {
            return BadRequest();
        }

        var products = new List<Product>();
        foreach ( var productId in order.ProductIds) 
        {            
            var product = await productRepository.GetAsync(productId);
            products.Add(product);
        }

        order.TotalPrice = products.Sum(x => x.Price);
        var orderDto = order.AsDto(products);
        return Ok(orderDto);
         
    }        

}









 
