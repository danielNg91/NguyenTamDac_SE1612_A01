using Api.Models;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Models;
using System.Linq.Expressions;
using System.Net;

namespace Api.Controllers;

public class FlowerBouquetController : BaseController
{
    private readonly IRepository<FlowerBouquet> _flowerRepository;
    private readonly IRepository<Category> _catgoryRepository;
    private readonly IRepository<Supplier> _supplierRepository;

    public FlowerBouquetController(IRepository<FlowerBouquet> flowerRepository,
        IRepository<Category> catgoryRepository,
        IRepository<Supplier> supplierRepository)
    {
        _flowerRepository = flowerRepository;
        _catgoryRepository = catgoryRepository;
        _supplierRepository = supplierRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetFlowerBouquets()
    {
        return Ok(await _flowerRepository.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateFlowerBouquet(CreateFlowerBouquet req)
    {
        var target = await _flowerRepository.FirstOrDefaultAsync(f => f.FlowerBouquetId == req.FlowerBouquetId);
        if (target != null)
        {
            throw new BadRequestException("Entity existed");
        }
        FlowerBouquet entity = Mapper.Map(req, new FlowerBouquet());
        await ValidateNavigations(entity);
        await _flowerRepository.CreateAsync(entity);
        return StatusCode(StatusCodes.Status201Created);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowerBouquet(int id)
    {
        var target = await _flowerRepository.FoundOrThrow(f => f.FlowerBouquetId == id, new NotFoundException("Entity not found"));
        return Ok(target);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowerBouquet(int id, UpdateFlowerBouquet req)
    {
        var target = await _flowerRepository.FoundOrThrow(f => f.FlowerBouquetId == id, new NotFoundException("Entity not found"));
        FlowerBouquet entity = Mapper.Map(req, target);
        await ValidateNavigations(entity);
        await _flowerRepository.UpdateAsync(entity);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowerBouquet(int id)
    {
        var target = await _flowerRepository.FoundOrThrow(f => f.FlowerBouquetId == id, new NotFoundException("Entity not found"));
        await _flowerRepository.DeleteAsync(target);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    private async Task ValidateCategory(int id)
    {
        await _catgoryRepository.FoundOrThrow(c => c.CategoryId == id, new BadRequestException("Category not found"));
    }

    private async Task ValidateSupplier(int id)
    {
        await _supplierRepository.FoundOrThrow(s => s.SupplierId == id, new BadRequestException("Supplier not found"));
    }

    private async Task ValidateNavigations(FlowerBouquet entity)
    {
        await ValidateCategory(entity.CategoryId);
        if (entity.SupplierId != null)
        {
            await ValidateSupplier((int)entity.SupplierId);
        }
    }
}
