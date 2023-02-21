using IdentityModel.Client;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Net.Http;
using ClientCredentialsFlow.Model;
using ClientCredentialsFlow.Services;

var client = new HttpClient()
{
    BaseAddress = new Uri("https://localhost:5001"),
};

var disco = await client.GetDiscoveryDocumentAsync();
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

var tokenRequest = new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client-secret",
    ClientSecret = "client-secret",
    Scope = "weather",
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
