using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Models;
using WebClient.Utils;

namespace WebClient.Controllers;

[Authorize(Roles = PolicyName.ADMIN)]
public class FlowersController : BaseController
{
    public FlowersController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
    }

    [Authorize(Roles = $"{PolicyName.ADMIN},{PolicyName.CUSTOMER}")]
    public async Task<IActionResult> Index()
    {
        var flowers = await ApiClient.GetAsync<List<FlowerBouquet>>($"{BaseUri}/{FlowersUrl}");
        return View(flowers);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Catgories"] = await GetCategoriesList();
        ViewData["Suppliers"] = await GetSuppliersList();
        return View();
    }

    private async Task<List<SelectListItem>> GetCategoriesList()
    {
        var categories = await ApiClient.GetAsync<List<Category>>($"{BaseUri}/{CategoriesUrl}");
        var selectListCategories = new List<SelectListItem>();
        foreach (var category in categories)
        {
            selectListCategories.Add(new SelectListItem { Text = category.CategoryName, Value = category.CategoryId.ToString() });
        }
        return selectListCategories;
    }

    private async Task<List<SelectListItem>> GetSuppliersList()
    {
        var suppliers = await ApiClient.GetAsync<List<Supplier>>($"{BaseUri}/{SuppliersUrl}");
        var selectListSuppliers = new List<SelectListItem>();
        foreach (var supplier in suppliers)
        {
            selectListSuppliers.Add(new SelectListItem { Text = supplier.SupplierName, Value = supplier.SupplierId.ToString() });
        }
        return selectListSuppliers;
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateFlowerBouquet req)
    {
        try
        {
            await ApiClient.PostAsync<object, CreateFlowerBouquet>($"{BaseUri}/{FlowersUrl}", req);
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Create");
        }
    }

    public async Task<IActionResult> Detail(int id)
    {
        var flower = await ApiClient.GetAsync<FlowerBouquet>($"{BaseUri}/{FlowersUrl}/{id}");
        return View(flower);
    }

    public async Task<IActionResult> Update(int id)
    {
        var flower = await ApiClient.GetAsync<FlowerBouquet>($"{BaseUri}/{FlowersUrl}/{id}");
        UpdateFlowerBouquet model = new UpdateFlowerBouquet
        {
            CategoryId = flower.CategoryId,
            FlowerBouquetName = flower.FlowerBouquetName,
            Description = flower.Description,
            UnitPrice = flower.UnitPrice,
            UnitsInStock = flower.UnitsInStock,
            FlowerBouquetStatus = flower.FlowerBouquetStatus,
            SupplierId = flower.SupplierId,
        };
        ViewData["Catgories"] = await GetCategoriesList();
        ViewData["Suppliers"] = await GetSuppliersList();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, UpdateFlowerBouquet req)
    {
        try
        {
            await ApiClient.PutAsync<object, UpdateFlowerBouquet>($"{BaseUri}/{FlowersUrl}/{id}", req);
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Update", new { id });
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var flower = await ApiClient.GetAsync<FlowerBouquet>($"{BaseUri}/{FlowersUrl}/{id}");
        return View(flower);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFlower(int id)
    {
        try
        {
            await ApiClient.DeleteAsync<object>($"{BaseUri}/{FlowersUrl}/{id}");
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Flower has ordered, cannot delete";
            return RedirectToAction("Delete", new { id });
        }
    }
}
