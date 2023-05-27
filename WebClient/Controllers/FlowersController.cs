using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Utils;

namespace WebClient.Controllers;

[Authorize(Roles = PolicyName.ADMIN)]
public class FlowersController : BaseController
{
    private string _flowersUrl { get; set; }

    public FlowersController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
        _flowersUrl = appSettings.Value.FlowersUrl;
    }

    [Authorize(Roles = $"{PolicyName.ADMIN},{PolicyName.CUSTOMER}")]
    public async Task<IActionResult> Index()
    {
        var flowers = await ApiClient.GetAsync<List<FlowerBouquet>>($"{BaseUri}/{_flowersUrl}");
        return View(flowers);
    }
}
