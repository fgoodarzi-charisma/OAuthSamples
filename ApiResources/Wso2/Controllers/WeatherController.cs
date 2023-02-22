using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wso2.Services;

namespace Wso2.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Weather>> Get()
    {
        var actorToken = await ClientCredentialsHelper.GetToken("api1", "api1");
        var subjectToken = ReadTokenFromHeader();
        var exchangedAccessToken = await TokenExchangeService.ExchangeForDelegation(subjectToken, actorToken.AccessToken);

        //var exchangedAccessToken = await TokenExchangeService.ExchangeForImpersonation(subjectToken);

        var simpleWeathers = SimpleWeather.Fake;
        var cities = await GeoService.GetLocations(exchangedAccessToken.TokenType, exchangedAccessToken.AccessToken);

        return simpleWeathers.Select(s => new Weather
        {
            Date = s.Date,
            Temperature = s.Temperature,
            City = cities.First(c => c.Id == s.CityId).Name,
        });
    }

    private string ReadTokenFromHeader()
    {
        var header = Request.Headers["Authorization"];
        return header.ToString().Split(" ")[1];
    }
}
