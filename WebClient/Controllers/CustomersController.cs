using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClient.Datasource;
using WebClient.Models;
using WebClient.Utils;

namespace WebClient.Controllers;

[Authorize(Roles = PolicyName.ADMIN)]
public class CustomersController : BaseController
{
    public CustomersController(IOptions<AppSettings> appSettings, IApiClient apiClient) : base(appSettings, apiClient)
    {
    }

    public async Task<IActionResult> Index()
    {
        var customers = await ApiClient.GetAsync<List<Customer>>($"{BaseUri}/{CustomersUrl}");
        return View(customers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomer createCustomer)
    {
        try
        {
            await ApiClient.PostAsync<object, CreateCustomer>($"{BaseUri}/{CustomersUrl}", createCustomer);
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Account already exist";
            return RedirectToAction("Create");
        }
    }

    public async Task<IActionResult> Detail(int id)
    {
        var customer = await ApiClient.GetAsync<Customer>($"{BaseUri}/{CustomersUrl}/{id}");
        return View(customer);
    }

    public async Task<IActionResult> Update(int id)
    {
        var customer = await ApiClient.GetAsync<Customer>($"{BaseUri}/{CustomersUrl}/{id}");
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
    public async Task<IActionResult> Update(int id, UpdateCustomer updateCustomer)
    {
        try
        {
            await ApiClient.PutAsync<object, UpdateCustomer>($"{BaseUri}/{CustomersUrl}/{id}", updateCustomer);
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Update", new { id });
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var customer = await ApiClient.GetAsync<Customer>($"{BaseUri}/{CustomersUrl}/{id}");
        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            await ApiClient.DeleteAsync<object>($"{BaseUri}/{CustomersUrl}/{id}");
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["Message"] = "Server Error";
            return RedirectToAction("Delete", new { id });
        }
    }
}
