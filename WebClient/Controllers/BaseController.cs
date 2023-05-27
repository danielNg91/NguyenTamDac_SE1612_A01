using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebClient.Controllers;
public class BaseController : Controller
{
    protected readonly string BaseUri;

    public BaseController(IOptions<AppSettings> appSettings)
    {
        BaseUri = appSettings.Value.BaseUri;
    }
}
