using IdentityModel.Client;
using Shared;
using System.Net.Http;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Weather.Services;

public static class TokenExchangeService
{
    public static async Task<IdentityModel.Client.TokenResponse> ExchangeForImpersonation(string token)
    {
        var cache = new DiscoveryCache(SampleConstants.StsBaseUrl);
        var disco = await cache.GetAsync();

        var tokenRequest = new TokenExchangeTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "smpl__weather_client",
            ClientSecret = "smpl__weather_client_secret",

            SubjectToken = token,
            SubjectTokenType = TokenTypeIdentifiers.AccessToken,
            Scope = "smpl__geography",
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestTokenExchangeTokenAsync(tokenRequest);

        return tokenResponse;
    }

    public static async Task<IdentityModel.Client.TokenResponse> ExchangeForDelegation(string subjectToken, string actorToken)
    {
        var cache = new DiscoveryCache(SampleConstants.StsBaseUrl);
        var disco = await cache.GetAsync();

        var tokenRequest = new TokenExchangeTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "smpl__weather_client",
            ClientSecret = "smpl__weather_client_secret",

            SubjectToken = subjectToken,
            SubjectTokenType = TokenTypeIdentifiers.AccessToken,
            Scope = "smpl__geography",

            ActorToken = actorToken,
            ActorTokenType = TokenTypeIdentifiers.AccessToken,
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestTokenExchangeTokenAsync(tokenRequest);

        return tokenResponse;
    }
}