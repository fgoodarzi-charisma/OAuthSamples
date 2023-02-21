using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Wso2.Services;

public static class TokenExchangeService
{
    public static async Task<IdentityModel.Client.TokenResponse> ExchangeForImpersonation(string token)
    {
        var cache = new DiscoveryCache("https://localhost:5001");
        var disco = await cache.GetAsync();

        var tokenRequest = new TokenExchangeTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "api1",
            ClientSecret = "api1",

            SubjectToken = token,
            SubjectTokenType = TokenTypeIdentifiers.AccessToken,
            Scope = "geography",
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestTokenExchangeTokenAsync(tokenRequest);

        return tokenResponse;
    }

    public static async Task<IdentityModel.Client.TokenResponse> ExchangeForDelegation(string subjectToken, string actorToken)
    {
        var cache = new DiscoveryCache("https://localhost:5001");
        var disco = await cache.GetAsync();

        var tokenRequest = new TokenExchangeTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "api1",
            ClientSecret = "api1",

            SubjectToken = subjectToken,
            SubjectTokenType = TokenTypeIdentifiers.AccessToken,
            Scope = "geography",

            ActorToken = actorToken,
            ActorTokenType = TokenTypeIdentifiers.AccessToken,
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestTokenExchangeTokenAsync(tokenRequest);

        return tokenResponse;
    }
}