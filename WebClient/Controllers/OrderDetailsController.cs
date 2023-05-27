using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Models;

namespace WebClient.Controllers;

public class OrderDetailsController : BaseController
{
    public OrderDetailsController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
    }

    private async Task<List<SelectListItem>> GetFlowersList()
    {
        var flowers = await ApiClient.GetAsync<List<FlowerBouquet>>($"{BaseUri}/{FlowersUrl}");
        var selectListCategories = new List<SelectListItem>();
        foreach (var flower in flowers)
        {
            selectListCategories.Add(new SelectListItem { Text = flower.FlowerBouquetName, Value = flower.FlowerBouquetId.ToString() });
        }
        return selectListCategories;
    }

    public async Task<IActionResult> Update(int orderId, int flowerId)
    {
        var detail = await ApiClient.GetAsync<OrderDetail>($"{BaseUri}/{OrdersUrl}/{orderId}/{OrderDetailsUrl}/{flowerId}");
        UpdateOrderDetail model = new UpdateOrderDetail
        {
            OrderId = detail.OrderId,
            FlowerBouquetId = detail.FlowerBouquetId,
            UnitPrice = detail.UnitPrice,
            Quantity = detail.Quantity,
            Discount = detail.Discount,
            FlowerBouquet = detail.FlowerBouquet
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int orderId, int flowerId, UpdateOrderDetail req)
    {
        try
        {
            await ApiClient.PutAsync<object, UpdateOrderDetail>($"{BaseUri}/{OrdersUrl}/{orderId}/{OrderDetailsUrl}/{flowerId}", req);
            return RedirectToAction("Detail", "Orders", new { id = orderId });
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Update", new { orderId, flowerId });
        }
    }

    public async Task<IActionResult> Delete(int orderId, int flowerId)
    {
        var detail = await ApiClient.GetAsync<OrderDetail>($"{BaseUri}/{OrdersUrl}/{orderId}/{OrderDetailsUrl}/{flowerId}");
        return View(detail);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrderDetail(int orderId, int flowerId)
    {
        try
        {
            await ApiClient.DeleteAsync<object>($"{BaseUri}/{OrdersUrl}/{orderId}/{OrderDetailsUrl}/{flowerId}");
            return RedirectToAction("Detail", "Orders", new { id = orderId });
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Delete", new { orderId, flowerId });
        }
    }
}
