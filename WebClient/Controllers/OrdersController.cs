using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Models;
using WebClient.Utils;

namespace WebClient.Controllers;

[Authorize(Roles = $"{PolicyName.ADMIN},{PolicyName.CUSTOMER}")]
public class OrdersController : BaseController
{
    public OrdersController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
    }

    public async Task<IActionResult> Index()
    {
        List<Order> orders;
        if (IsAdmin)
        {
            orders = await ApiClient.GetAsync<List<Order>>($"{BaseUri}/{OrdersUrl}");
        }
        else
        {
            orders = await ApiClient.GetAsync<List<Order>>($"{BaseUri}/{OrdersUrl}?id={CurrentUserId}");
        }
        return View(orders);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var order = await ApiClient.GetAsync<Order>($"{BaseUri}/{OrdersUrl}/{id}");
        return View(order);
    }

    public async Task<IActionResult> Update(int id)
    {
        var order = await ApiClient.GetAsync<Order>($"{BaseUri}/{OrdersUrl}/{id}");
        UpdateOrder model = new UpdateOrder
        {
            OrderId = order.OrderId,
            CustomerId = (int)order.CustomerId,
            OrderDate = order.OrderDate,
            ShippedDate = order.ShippedDate,
            Total = order.Total,
            OrderStatus = order.OrderStatus,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, UpdateOrder req)
    {
        try
        {
            await ApiClient.PutAsync<object, UpdateOrder>($"{BaseUri}/{OrdersUrl}/{id}", req);
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
        var order = await ApiClient.GetAsync<Order>($"{BaseUri}/{OrdersUrl}/{id}");
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            await ApiClient.DeleteAsync<object>($"{BaseUri}/{OrdersUrl}/{id}");
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Delete", new { id });
        }
    }
}
