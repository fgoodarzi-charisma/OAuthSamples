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
            ClientId = SampleConstants.Client_WeatherClientId,
            ClientSecret = SampleConstants.Client_WeatherClientSecret,

            SubjectToken = token,
            SubjectTokenType = TokenTypeIdentifiers.AccessToken,
            Scope = SampleConstants.GeographyScope,
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
            ClientId = SampleConstants.Client_WeatherClientId,
            ClientSecret = SampleConstants.Client_WeatherClientSecret,

            SubjectToken = subjectToken,
            SubjectTokenType = TokenTypeIdentifiers.AccessToken,
            Scope = SampleConstants.GeographyScope,

            ActorToken = actorToken,
            ActorTokenType = TokenTypeIdentifiers.AccessToken,
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestTokenExchangeTokenAsync(tokenRequest);

        return tokenResponse;
    }
}