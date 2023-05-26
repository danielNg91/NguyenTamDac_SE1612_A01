using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebClient.Controllers;


public class LoginController : Controller
{
    public LoginController()
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login()
    {
        return Ok("test");
    }
}
