using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Models;

namespace WebClient.Controllers;
public class RegisterController : BaseController
{

    public RegisterController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterAccount registerAccount)
    {
        try
        {
            await ApiClient.PostAsync<string, RegisterAccount>($"{BaseUri}/{RegisterUrl}", registerAccount);
            return RedirectToAction("Index", "Login");
        }
        catch
        {
            TempData["Message"] = "Account already exist";
            return RedirectToAction("Index");
        }
    }
}
