using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebClient.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        var auth = await HttpContext.AuthenticateAsync();

        //var tokenType = rs.Properties?.GetTokenValue(".access-token-type");
        //var accessToken = rs.Properties?.GetTokenValue(".access-token");
        //var weatherForecasts = await WeatherService.GetWeatherForecasts(tokenType, accessToken);
        //var user = User;
    }
}
