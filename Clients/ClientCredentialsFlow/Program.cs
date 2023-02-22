using Shared.Models;
using Shared.Services;
using IdentityModel.Client;
using Spectre.Console;
using System.Security.Cryptography.X509Certificates;

// discover endpoints from metadata
var handler = new SocketsHttpHandler();
var path = $"{Environment.CurrentDirectory}/cert-11.pfx";
var cert = new X509Certificate2(path, "fF@123456");
handler.SslOptions.ClientCertificates = new X509CertificateCollection { cert };

var client = new HttpClient(handler);

var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.MtlsEndpointAliases.TokenEndpoint,
    ClientId = "client-credentials",
    Scope = "weather",
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine(tokenResponse.ErrorDescription);
    return;
}

var weatherForecasts = await WeatherService.GetWeatherForecasts(tokenResponse);

DisplayForecasts(weatherForecasts);

Console.ReadLine();

static void DisplayForecasts(List<WeatherForecast> forecasts)
{
    var table = new Table();
    table.AddColumn("City");
    table.AddColumn("Temperature");
    table.AddColumn("Date");

    foreach (var forecast in forecasts)
    {
        table.AddRow(forecast.City, forecast.Temperature.ToString(), forecast.Date.ToShortDateString());
    }
    AnsiConsole.Write(table);
}