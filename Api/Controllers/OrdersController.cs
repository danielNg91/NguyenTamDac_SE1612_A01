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
    private readonly IRepository<OrderDetail> _oderDetailRepository;

    public OrdersController(IRepository<Order> orderRepository, IRepository<OrderDetail> oderDetailRepository)
    {
        _orderRepository = orderRepository;
        _oderDetailRepository = oderDetailRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(DateTime? startDate, DateTime? endDate, int? id)
    {
        IOrderedEnumerable<Order> orders;

        if (startDate == null && endDate == null)
        {
            orders = (await _orderRepository.ToListAsync()).OrderByDescending(c => c.Total);
        } else
        {
            if (startDate != null && endDate == null)
            {
                endDate = startDate;
            } else if (startDate == null && endDate != null)
            {
                startDate = endDate;
            }
            
            if (DateTime.Compare((DateTime)startDate, (DateTime)endDate) > 0)
            {
                throw new BadRequestException("StartDate cannot be later than EndDate");
            }
            orders = (await _orderRepository.WhereAsync(
                o => DateTime.Compare(o.OrderDate, (DateTime)startDate) >= 0 && DateTime.Compare(o.OrderDate, (DateTime)endDate) <= 0)
                ).OrderByDescending(c => c.Total);
        }

        if (id != null)
        {
            orders = orders.Where(o => o.CustomerId == id).OrderByDescending(c => c.Total);
        }
        return Ok(orders);
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
        var target = await _orderRepository.FirstOrDefaultAsync(c => c.OrderId == id);
        if (target == null)
        {
            throw new NotFoundException();
        }
        var details = await _oderDetailRepository.WhereAsync(d => d.OrderId == id, new string[] { nameof(FlowerBouquet) });
        target.OrderDetails = details;
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

    [HttpGet("{id}/order-details")]
    public async Task<IActionResult> GetOrderDetails(int id)
    {
        var target = await _oderDetailRepository.FirstOrDefaultAsync(
            c => c.OrderId == id, new string[] { nameof(FlowerBouquet) }
            );
        if (target == null)
        {
            throw new NotFoundException();
        }
        return Ok(target);
    }
}
