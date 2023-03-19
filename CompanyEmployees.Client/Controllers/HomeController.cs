using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CompanyEmployees.Client.Models;
using Microsoft.AspNetCore.Authorization;

namespace CompanyEmployees.Client.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory; 


    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [Authorize]
    public async Task<IActionResult> Companies() 
    { 
        var httpClient = _httpClientFactory.CreateClient("APIClient"); 
 
        var response = await httpClient.GetAsync("api/companies").ConfigureAwait(false); 
 
        response.EnsureSuccessStatusCode(); 
 
        var companiesString = await response.Content.ReadAsStringAsync(); 
        var companies = JsonSerializer.Deserialize<List<CompanyViewModel>>(companiesString, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true}); 
 
        return View(companies); 
    }

    public IActionResult Edit()
    {
        throw new NotImplementedException();
    }

    public IActionResult Delete()
    {
        throw new NotImplementedException();
    }

    public IActionResult Details()
    {
        throw new NotImplementedException();
    }

    public IActionResult Create()
    {
        throw new NotImplementedException();
    }
}