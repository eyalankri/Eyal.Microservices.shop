using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using Orders.Entities;

namespace Orders.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;

    public OrderController(IRepository<Order> orderRepository, IRepository<Product> productRepository)
    {
        this._orderRepository = orderRepository;
        this._productRepository = productRepository;
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

        await _orderRepository.CreateAsync(order);
        return Ok();
    }

    [HttpGet]
    [Route("order/{orderId}")]
    public async Task<ActionResult<OrderDto>> GetAsync(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            return BadRequest();
        }
        var order = await _orderRepository.GetAsync(orderId);
        if (order.ProductIds?.Count == 0)
        {
            return BadRequest();
        }

        var products = new List<Product>();
        foreach (var productId in order.ProductIds!)
        {
            var product = await _productRepository.GetAsync(productId);
            products.Add(product);
        }

        order.TotalPrice = products.Sum(x => x.Price);
        var orderDto = order.AsDto(products);
        return Ok(orderDto);

    }


    [HttpGet]
    [Route("orders")]
    public async Task<ActionResult<IReadOnlyCollection<OrderDto>>> GetAllAsync()
    {

        var orderDtoList = new List<OrderDto>();

        var orders = await _orderRepository.GetAllAsync();
        foreach (var order in orders)
        {
            var products = new List<Product>();
            foreach (var productId in order.ProductIds!)
            {
                var product = await _productRepository.GetAsync(productId);
                products.Add(product);
            }
            order.TotalPrice = products.Sum(x => x.Price);
            var orderDto = order.AsDto(products);
            orderDtoList.Add(orderDto);
        }
        
        return Ok(orderDtoList);
    }
}










