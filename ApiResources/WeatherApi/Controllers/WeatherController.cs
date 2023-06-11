using Microsoft.AspNetCore.Mvc;
using MtlsClientCredentitals.Models;
using MtlsClientCredentitals.Services;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Services;

namespace Weather.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class WeatherController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<MtlsClientCredentitals.Models.Weather>> Get()
    {
        var actorToken = await ClientCredentialsHelper.GetToken(SampleConstants.Client_WeatherClientId,
            SampleConstants.Client_WeatherClientSecret);
        var subjectToken = ReadTokenFromHeader();

        //var exchangedAccessToken = await TokenExchangeService.ExchangeForDelegation(subjectToken, actorToken.AccessToken);
        var exchangedAccessToken = await TokenExchangeService.ExchangeForImpersonation(subjectToken);

        var simpleWeathers = SimpleWeather.Fake;
        var cities = await GeoService.GetLocations(exchangedAccessToken.TokenType, exchangedAccessToken.AccessToken);

        return simpleWeathers.Select(s => new MtlsClientCredentitals.Models.Weather
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
