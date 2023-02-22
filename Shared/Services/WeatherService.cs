using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Shared.Models;

namespace Shared.Services;

public static class WeatherService
{
    public static async Task<List<Weather>> GetWeathers(string scheme, string token)
    {
        var client = new RestClient($"https://localhost:5005/");
        client.UseNewtonsoftJson();
        var request = new RestRequest("/weather", Method.Get);
        request.AddHeader("Authorization", $"{scheme} {token}");
        var response = await client.ExecuteAsync<List<Weather>>(request);
        return response.Data!;
    }
}
