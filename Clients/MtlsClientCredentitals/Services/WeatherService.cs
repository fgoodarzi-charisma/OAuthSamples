using IdentityModel.Client;
using MtlsClientCredentitals.Models;
using RestSharp;
using Shared;

namespace MtlsClientCredentitals.Services;

public static class WeatherService
{
    public static async Task<List<WeatherForecast>> GetWeatherForecasts(TokenResponse token)
    {
        var client = new RestClient(SampleConstants.WeatheApiBaseUrl);
        var request = new RestRequest("/WeatherForecast", Method.Get);
        request.AddHeader("Authorization", $"{token.TokenType} {token.AccessToken}");
        var response = await client.ExecuteAsync<List<WeatherForecast>>(request);
        return response.Data;
    }

}