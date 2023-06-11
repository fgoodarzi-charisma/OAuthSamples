using MtlsClientCredentitals.Models;
using RestSharp;
using Shared;

namespace MtlsClientCredentitals.Services;

public static class WeatherService
{
    public static async Task<List<Weather>> GetWeathers(string scheme, string token)
    {
        var client = new RestClient(SampleConstants.WeatheApiBaseUrl);
        var request = new RestRequest("/weather", Method.Get);
        request.AddHeader("Authorization", $"{scheme} {token}");
        var response = await client.ExecuteAsync<List<Weather>>(request);
        return response.Data!;
    }
}
