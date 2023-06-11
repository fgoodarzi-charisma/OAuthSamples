using MtlsClientCredentitals.Models;
using RestSharp;
using Shared;

namespace MtlsClientCredentitals.Services;

public static class GeoService
{
    public static async Task<List<City>> GetLocations(string scheme, string token)
    {
        var client = new RestClient(SampleConstants.GeographyApiBaseUrl);
        var request = new RestRequest("/city", Method.Get);
        request.AddHeader("Authorization", $"{scheme} {token}");
        var response = await client.ExecuteAsync<List<City>>(request);
        return response.Data!;
    }
}