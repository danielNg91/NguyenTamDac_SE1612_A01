using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Utils;

namespace WebClient.Controllers;

[Authorize(Roles = PolicyName.ADMIN)]
public class CustomersController : BaseController
{
    private string _customersUrl { get; set; }

    public CustomersController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
        _customersUrl = appSettings.Value.CustomersUrl;
    }

    public async Task<IActionResult> Index()
    {
        var customers = await ApiClient.GetAsync<List<Customer>>($"{BaseUri}/{_customersUrl}");
        return View(customers);
    }
}
