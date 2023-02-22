using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Shared.Models;

namespace Shared.Services;

public static class GeoService
{
    public static async Task<List<City>> GetLocations(string scheme, string token)
    {
        var client = new RestClient($"https://localhost:5006/");
        client.UseNewtonsoftJson();
        var request = new RestRequest("/city", Method.Get);
        request.AddHeader("Authorization", $"{scheme} {token}");
        var response = await client.ExecuteAsync<List<City>>(request);
        return response.Data!;
    }
}