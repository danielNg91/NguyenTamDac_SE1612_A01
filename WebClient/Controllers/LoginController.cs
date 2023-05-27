using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Models;

namespace WebClient.Controllers;


public class LoginController : BaseController
{
    private readonly IApiClient _apiClient;
    public string LoginUrl { get; set; }

    public LoginController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings)
    {
        _apiClient = apiClient;
        LoginUrl = appSettings.Value.LoginUrl;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginCredentials credentials)
    {
        try
        {
            await _apiClient.PostAsync($"{BaseUri}/{LoginUrl}", credentials);
            return RedirectToAction("Index");
        } catch (Exception e)
        {
            //throw new 
        }
    }
}
