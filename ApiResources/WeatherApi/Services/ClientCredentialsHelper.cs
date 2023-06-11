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
            Scope = "smpl__geography",
        };

        var client = new HttpClient();

        return await client.RequestClientCredentialsTokenAsync(actorRequest);
    }
}