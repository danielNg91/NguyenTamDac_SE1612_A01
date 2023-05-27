using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Utils;

namespace WebClient.Controllers;
public class BaseController : Controller
{
    protected readonly string BaseUri;
    protected readonly string FlowersUrl;
    protected readonly string CategoriesUrl;
    protected readonly string SuppliersUrl;
    protected readonly string CustomersUrl;
    protected readonly string OrdersUrl;
    protected readonly string LoginUrl;
    protected readonly string RegisterUrl;
    public int CurrentUserId => GetCurrentUserId();

    public bool IsAdmin => IsCurrentUserAdmin();
    private bool IsCurrentUserAdmin()
    {
        return User.IsInRole(nameof(Role.Admin));
    }

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
        OrdersUrl = appSettings.Value.OrdersUrl;
        ApiClient = apiClient;
    }

    private int GetCurrentUserId()
    {
        if (!User.Identity.IsAuthenticated)
        {
            Redirect("Login");
            return 0;
        }
        return int.Parse(User.FindFirst(x => x.Type == "id").Value);
    }
}
