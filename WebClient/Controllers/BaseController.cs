using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;

namespace WebClient.Controllers;
public class BaseController : Controller
{
    protected readonly string BaseUri;
    protected readonly string FlowersUrl;
    protected readonly string CategoriesUrl;
    protected readonly string SuppliersUrl;
    protected readonly string CustomersUrl;
    protected readonly string LoginUrl;
    protected readonly string RegisterUrl;


    protected readonly IApiClient ApiClient;

    public BaseController(IOptions<AppSettings> appSettings, IApiClient apiClient)
    {
        BaseUri = appSettings.Value.BaseUri;
        FlowersUrl = appSettings.Value.FlowersUrl;
        CategoriesUrl = appSettings.Value.CategoriesUrl;
        SuppliersUrl = appSettings.Value.SuppliersUrl;
        CustomersUrl = appSettings.Value.CustomersUrl;
        LoginUrl = appSettings.Value.LoginUrl;
        RegisterUrl = appSettings.Value.RegisterUrl;
        ApiClient = apiClient;
    }
}
