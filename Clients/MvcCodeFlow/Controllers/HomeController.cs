using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MvcCodeFlow.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [AllowAnonymous]
    public IActionResult Index() => View();

    public IActionResult Secure() => View();

    [AllowAnonymous]
    public IActionResult Insecure() => View();

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> CallApi()
    {
        var client = _httpClientFactory.CreateClient("smpl__weather_api");
        var response = await client.GetStringAsync("weather");

        var json = JsonDocument.Parse(response);
        ViewBag.Json = JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });

        return View();
    }
}