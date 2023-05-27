using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using WebClient.Datasource;

namespace WebClient.Controllers;
public class BaseController : Controller
{
    protected readonly string BaseUri;
    protected readonly IApiClient ApiClient;

    public BaseController(IOptions<AppSettings> appSettings, IApiClient apiClient)
    {
        BaseUri = appSettings.Value.BaseUri;
        ApiClient = apiClient;
    }
}
