using Api;
using Api.Utils;
using AutoWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

services.AddAutoMapper(Assembly.GetExecutingAssembly());

services.AddAppAuthentication();
services.AddAppAuthorization();

services.AddDbContext<FUFlowerBouquetManagementContext>(options =>
{
    {
        var settings = services.BuildServiceProvider().GetService<IOptions<AppSettings>>();
        Console.WriteLine(settings);
        options.UseSqlServer(settings.Value.ConnectionStrings.FUFlowerBouquetManagement);
    }
});

services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
services.AddTransient<AuthenticationEvent>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = false, ShowIsErrorFlagForSuccessfulResponse = true });


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
