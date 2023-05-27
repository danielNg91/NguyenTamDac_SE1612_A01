using Api.Models;
using Api.Utils;
using Application.Exceptions;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Api.Controllers;


[Route("api/v1/orders")]
public class OrdersController : BaseController
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderDetail> _oderDetailRepository;
    private readonly IRepository<FlowerBouquet> _flowerRepository;

    public OrdersController(
        IRepository<Order> orderRepository, 
        IRepository<OrderDetail> oderDetailRepository, 
        IRepository<FlowerBouquet> flowerRepository)
    {
        _orderRepository = orderRepository;
        _oderDetailRepository = oderDetailRepository;
        _flowerRepository = flowerRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(DateTime? startDate, DateTime? endDate, int? id)
    {
        IOrderedEnumerable<Order> orders;

        if (id != null)
        {
            orders = (await _orderRepository.WhereAsync(o => o.CustomerId == id)).OrderByDescending(c => c.Total);
        } else
        {
            orders = (await _orderRepository.ToListAsync()).OrderByDescending(c => c.Total);
        }

        if (startDate != null || endDate != null)
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
            orders = orders.Where(
                o => DateTime.Compare(o.OrderDate, (DateTime)startDate) >= 0 && DateTime.Compare(o.OrderDate, (DateTime)endDate) <= 0
                ).OrderByDescending(c => c.Total);
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
        List<OrderDetail> orderDetails = new List<OrderDetail>();
        foreach(var flowerReq in req.Flowers)
        {
            var flower = await _flowerRepository.FoundOrThrow(
                f => f.FlowerBouquetId == flowerReq.FlowerBouquetId, new BadRequestException("FlowerId not found")
                );
            orderDetails.Add(new OrderDetail
            {
                FlowerBouquetId = flower.FlowerBouquetId,
                OrderId = req.OrderId,
                Discount = 0,
                Quantity = flowerReq.Quantity,
                UnitPrice = flower.UnitPrice,
            });
        }

        await _orderRepository.CreateAsync(entity);
        await _oderDetailRepository.CreateAsync(orderDetails);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var target = await _orderRepository.FirstOrDefaultAsync(c => c.OrderId == id, new string[] { "OrderDetails" });
        if (target == null)
        {
            throw new NotFoundException();
        }
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

    [HttpGet("{orderId}/order-details")]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
        var target = await _oderDetailRepository.WhereAsync(
            c => c.OrderId == orderId, new string[] { nameof(FlowerBouquet) }
            );
        if (target == null)
        {
            throw new NotFoundException();
        }
        return Ok(target);
    }

    [HttpGet("{orderId}/order-details/{flowerId}")]
    public async Task<IActionResult> GetOrderDetails(int orderId, int flowerId)
    {
        var detail = await _oderDetailRepository.FirstOrDefaultAsync(
            c => c.OrderId == orderId && c.FlowerBouquetId == flowerId, new string[] { nameof(FlowerBouquet) }
            );
        if (detail == null)
        {
            throw new NotFoundException();
        }
        return Ok(detail);
    }

    [HttpPut("{orderId}/order-details/{flowerId}")]
    public async Task<IActionResult> GetOrderDetails(int orderId, int flowerId, UpdateOrderDetail req)
    {
        var detail = await _oderDetailRepository.FoundOrThrow(c => c.OrderId == orderId && c.FlowerBouquetId == flowerId, new NotFoundException());
        var entity = Mapper.Map(req, detail);
        await _oderDetailRepository.UpdateAsync(entity);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{orderId}/order-details/{flowerId}")]
    public async Task<IActionResult> DeleteOrderDetail(int orderId, int flowerId)
    {
        var detail = await _oderDetailRepository.FoundOrThrow(c => c.OrderId == orderId && c.FlowerBouquetId == flowerId, new NotFoundException());
        await _oderDetailRepository.DeleteAsync(detail);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
