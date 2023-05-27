using Api.Models;
using Api.Utils;
using Application.Exceptions;
using BusinessObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repository;
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
    public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
    {
        if (credentials.Email.Equals(_appSettings.Value.AdminAccount.Email) &&
            credentials.Password.Equals(_appSettings.Value.AdminAccount.Password))
        {
            var admin = new LoginResponse
            {
                CustomerId = 999,
                Email = credentials.Email,
                Role = PolicyName.ADMIN
            };
            //await SetIdentity("9999", _appSettings.Value.AdminAccount.Email, PolicyName.ADMIN);
            return Ok(admin);
        }

        var user = await _userRepository.FoundOrThrow(
            u => u.Email.Equals(credentials.Email) && u.Password.Equals(credentials.Password),
            new ForbiddenException());
        var response = Mapper.Map(user, new LoginResponse());
        response.Role = PolicyName.CUSTOMER;
        //await SetIdentity(user.CustomerId.ToString(), user.Email, PolicyName.CUSTOMER);
        return Ok(response);
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
            new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = true });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAccount req)
    {
        await ValidateRegisterFields(req);
        var user = Mapper.Map(req, new Customer());
        user.CustomerId = await GetUserId();
        await _userRepository.CreateAsync(user);
        return Ok();
    }

    private async Task<int> GetUserId()
    {
        var user = (await _userRepository.ToListAsync()).OrderByDescending(u => u.CustomerId).FirstOrDefault();
        return user == null ? 1 : (user.CustomerId + 1);
    }

    private async Task ValidateRegisterFields(RegisterAccount req)
    {
        if (req.Email.Equals(_appSettings.Value.AdminAccount.Email))
        {
            throw new BadRequestException("Email already existed");
        }

        var isEmailExisted = (await _userRepository.FirstOrDefaultAsync(u => u.Email == req.Email)) != null;
        if (isEmailExisted)
        {
            throw new BadRequestException("Email already existed");
        }
    }
}
