using Api.Models;
using Api.Utils;
using Application.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Models;
using System.Security.Claims;

namespace Api.Controllers;


[Route("api/v1/auth")]
public class AuthController : BaseController
{
    private readonly IRepository<Customer> _userRepository;
    private readonly IOptions<AppSettings> _appSettings;

    public AuthController(IOptions<AppSettings> appSettings, IRepository<Customer> userRepository)
    {
        _appSettings = appSettings;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginCredentials credentials)
    {
        if (credentials.Email.Equals(_appSettings.Value.AdminAccount.Email) &&
            credentials.Password.Equals(_appSettings.Value.AdminAccount.Password))
        {
            await SetIdentity("9999", _appSettings.Value.AdminAccount.Email, PolicyName.ADMIN);
            return Ok();
        }

        var user = await _userRepository.FoundOrThrow(
            u => u.Email.Equals(credentials.Email) && u.Password.Equals(credentials.Password), 
            new ForbiddenException());
        await SetIdentity(user.CustomerId.ToString(), user.Email, PolicyName.CUSTOMER);
        return Ok();
    }

    private async Task SetIdentity(string userId, string email, string role) 
    {
        var claims = new List<Claim>
                    {
                        new Claim("id", userId),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, role)
                    };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
    }
}
