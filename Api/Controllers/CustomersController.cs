using Api.Models;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Models;

namespace Api.Controllers;


[Route("api/v1/customers")]
public class CustomersController : BaseController
{
    private readonly IRepository<Customer> _customerRepository;

    public CustomersController(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        return Ok(await _customerRepository.ToListAsync());
    }


    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomer req)
    {
        var target = await _customerRepository.FirstOrDefaultAsync(c => c.CustomerId == req.CustomerId);
        if (target != null)
        {
            throw new BadRequestException("Entity existed");
        }
        Customer entity = Mapper.Map(req, new Customer());
        await _customerRepository.CreateAsync(entity);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var target = await _customerRepository.FoundOrThrow(c => c.CustomerId == id, new NotFoundException());
        return Ok(target);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomer req)
    {
        var target = await _customerRepository.FoundOrThrow(c => c.CustomerId == id, new NotFoundException());
        Customer entity = Mapper.Map(req, target);
        await _customerRepository.UpdateAsync(entity);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var target = await _customerRepository.FoundOrThrow(c => c.CustomerId == id, new NotFoundException());
        await _customerRepository.DeleteAsync(target);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
