using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
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
}
