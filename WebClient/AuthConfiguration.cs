using Microsoft.AspNetCore.Authentication.Cookies;
using WebClient.Utils;

namespace WebClient;

public static class AuthConfiguration
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyName.ADMIN,
                policy => policy.RequireRole(PolicyName.ADMIN));
            options.AddPolicy(PolicyName.CUSTOMER,
                policy => policy.RequireRole(PolicyName.CUSTOMER));
        });
        return services;
    }

    public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
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
            options.LoginPath = "/Login/Index";
            options.AccessDeniedPath = "/Home/Error403";
        });
        return services;
    }    
}
