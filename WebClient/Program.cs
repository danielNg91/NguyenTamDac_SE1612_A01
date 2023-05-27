using WebClient;
using WebClient.Datasource;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddAppAuthentication();
services.AddAppAuthorization();

services.AddControllersWithViews();

services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
services.AddScoped(typeof(IApiClient), typeof(ApiClient));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
