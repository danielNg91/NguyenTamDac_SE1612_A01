using Api.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Api;

public static class AuthConfiguration
{
    public static void AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyName.ADMIN,
                policy => policy.RequireRole(PolicyName.ADMIN));
            options.AddPolicy(PolicyName.CUSTOMER,
                policy => policy.RequireRole(PolicyName.CUSTOMER));
        });
    }
    public static void AddAppAuthentication(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.ExpireTimeSpan = System.TimeSpan.FromDays(1);
            options.Cookie.HttpOnly = true;
            options.EventsType = typeof(AuthenticationEvent);
        });
    }
}
