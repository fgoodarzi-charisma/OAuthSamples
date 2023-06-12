using IdentityModel.Client;
using Shared;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather.Services;

public static class ClientCredentialsHelper
{
    public static async Task<TokenResponse> GetToken(string username, string password)
    {
        var cache = new DiscoveryCache(SampleConstants.StsBaseUrl);
        var disco = await cache.GetAsync();

        var actorRequest = new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = username,
            ClientSecret = password,
            Scope = SampleConstants.GeographyScope,
        };

        var client = new HttpClient();

        return await client.RequestClientCredentialsTokenAsync(actorRequest);
    }

    public static async Task<TokenIntrospectionResponse> IntrospectToken(string token)
    {
        var cache = new DiscoveryCache(SampleConstants.StsBaseUrl);
        var disco = await cache.GetAsync();
        var client = new HttpClient();

        var response = await client.IntrospectTokenAsync(new TokenIntrospectionRequest
        {
            Address = disco.IntrospectionEndpoint,
            ClientId = SampleConstants.Api_WeatherId,
            ClientSecret = SampleConstants.Api_WeatherSecret,
            Token = token,
        });

        return response;
    }
}