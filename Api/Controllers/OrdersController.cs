using Api.Models;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Models;

namespace Api.Controllers;

[Route("api/v1/orders")]
public class OrdersController : BaseController
{
    private readonly IRepository<Order> _orderRepository;

    public OrdersController(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        return Ok(await _orderRepository.ToListAsync());
    }


    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrder req)
    {
        var target = await _orderRepository.FirstOrDefaultAsync(c => c.OrderId == req.OrderId);
        if (target != null)
        {
            throw new BadRequestException("Entity existed");
        }
        Order entity = Mapper.Map(req, new Order());
        entity.CustomerId = CurrentUserID;
        await _orderRepository.CreateAsync(entity);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var target = await _orderRepository.FoundOrThrow(c => c.OrderId == id, new NotFoundException());
        return Ok(target);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrder req)
    {
        var target = await _orderRepository.FoundOrThrow(c => c.OrderId == id, new NotFoundException());
        Order entity = Mapper.Map(req, target);
        await _orderRepository.UpdateAsync(entity);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var target = await _orderRepository.FoundOrThrow(c => c.OrderId == id, new NotFoundException());
        await _orderRepository.DeleteAsync(target);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
