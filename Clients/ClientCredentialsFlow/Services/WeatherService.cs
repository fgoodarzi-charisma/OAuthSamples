using IdentityModel.Client;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Shared.Models;

namespace Shared.Services;

public static class WeatherService
{
    public static async Task<List<WeatherForecast>> GetWeatherForecasts(TokenResponse token)
    {
        var client = new RestClient($"https://localhost:5005/");
        client.UseNewtonsoftJson();
        var request = new RestRequest("/WeatherForecast", Method.Get);
        request.AddHeader("Authorization", $"{token.TokenType} {token.AccessToken}");
        var response = await client.ExecuteAsync<List<WeatherForecast>>(request);
        return response.Data;
    }

}