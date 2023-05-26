using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers;
public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
