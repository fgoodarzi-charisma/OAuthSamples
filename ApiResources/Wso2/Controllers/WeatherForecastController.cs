using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wso2.Services;

namespace Wso2.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var actorToken = await ClientCredentialsHelper.GetToken("api1", "api1");
        var subjectToken = ReadTokenFromHeader();

        var newToken = await TokenExchangeService.ExchangeForDelegation(subjectToken, actorToken.AccessToken);
        //var newToken = await TokenExchangeService.ExchangeForImpersonation(subjectToken);

        return null;
    }

    private string ReadTokenFromHeader()
    {
        var header = Request.Headers["Authorization"];
        return header.ToString().Split(" ")[1];
    }
}
