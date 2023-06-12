using IdentityModel.Client;
using MtlsClientCredentitals.Models;
using MtlsClientCredentitals.Services;
using Shared;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var handler = new SocketsHttpHandler();
var path = $"{Environment.CurrentDirectory}/certificate.pfx";
var cert = new X509Certificate2(path, "smpl__client_credentials");
handler.SslOptions.ClientCertificates = new X509CertificateCollection { cert };

var client = new HttpClient()
{
    BaseAddress = new Uri(SampleConstants.StsBaseUrl),
};

var disco = await client.GetDiscoveryDocumentAsync();
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

if (disco.MtlsEndpointAliases is null)
{
    Console.WriteLine("Identity provider does not support MTLS");
    return;
}

var tokenRequest = new ClientCredentialsTokenRequest
{
    Address = disco.MtlsEndpointAliases.TokenEndpoint,
    ClientId = SampleConstants.Client_ClientCredentialsId,
    Scope = SampleConstants.WeatherScope,
};

var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest);

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine(tokenResponse.ErrorDescription);
    return;
}

Console.WriteLine("Access Token : ");
Console.WriteLine(tokenResponse.AccessToken);
Console.WriteLine("---------------------------");

var weather = await WeatherService.GetWeathers(tokenResponse.TokenType, tokenResponse.AccessToken);

DisplayForecasts(weather);

Console.ReadLine();

static void DisplayForecasts(List<Weather> forecasts)
{
    if (forecasts is null || !forecasts.Any())
    {
        return;
    }

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
