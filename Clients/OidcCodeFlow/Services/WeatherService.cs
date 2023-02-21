using IdentityModel.Client;
using ClientCredentialsFlow.Model;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace ClientCredentialsFlow.Services;

public static class WeatherService
{
    public static async Task<List<WeatherForecast>> GetWeatherForecasts(string tokenType, string token)
    {
        var client = new RestClient($"https://localhost:5005/");
        client.UseNewtonsoftJson();
        var request = new RestRequest("/WeatherForecast", Method.Get);
        request.AddHeader("Authorization", $"{tokenType} {token}");
        var response = await client.ExecuteAsync<List<WeatherForecast>>(request);
        return response.Data!;
    }

}