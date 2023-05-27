using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Models;
using WebClient.Utils;

namespace WebClient.Controllers;

[Authorize(Roles = PolicyName.CUSTOMER)]
public class ProfileController : BaseController
{
    public ProfileController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
    }

    public async Task<IActionResult> Index()
    {
        var customer = await ApiClient.GetAsync<Customer>($"{BaseUri}/{CustomersUrl}/{CurrentUserId}");
        return View(customer);
    }

    public async Task<IActionResult> Update()
    {
        var customer = await ApiClient.GetAsync<Customer>($"{BaseUri}/{CustomersUrl}/{CurrentUserId}");
        UpdateCustomer model = new UpdateCustomer
        {
            Email = customer.Email,
            CustomerName = customer.CustomerName,
            City = customer.City,
            Country = customer.Country,
            Password = customer.Password,
            Birthday = customer.Birthday,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCustomer updateCustomer)
    {
        try
        {
            await ApiClient.PutAsync<object, UpdateCustomer>($"{BaseUri}/{CustomersUrl}/{CurrentUserId}", updateCustomer);
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Update", new { CurrentUserId });
        }
    }
}
