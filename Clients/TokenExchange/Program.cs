using IdentityModel;
using IdentityModel.Client;
using Shared;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TokenExchange;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "Console Token Exchange Client";
DiscoveryCache cache = new(SampleConstants.StsBaseUrl);

// initial token
var response = await RequestTokenAsync(SampleConstants.Client_ClientCredentialsId,
    SampleConstants.Client_ClientCredentialsSecret, SampleConstants.WeatherScope);
var initialToken = response.AccessToken;

"\n\nInitial token:".ConsoleYellow();
response.Show();
Console.ReadLine();

response = await ExchangeToken(initialToken, "impersonation");

"\n\nImpersonation style:".ConsoleYellow();
response.Show();
Console.ReadLine();

response = await ExchangeToken(initialToken, "delegation");

"\n\nDelegation style:".ConsoleYellow();
response.Show();
Console.ReadLine();

async Task<TokenResponse> RequestTokenAsync(string clientId, string secret, string scope)
{
    var client = new HttpClient();

    var disco = await cache.GetAsync();
    if (disco.IsError) throw new Exception(disco.Error);

    var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,
        ClientId = clientId,
        ClientSecret = secret,
        Scope = scope,
    });

    if (response.IsError) throw new Exception(response.Error);
    return response;
}

async Task<TokenResponse> ExchangeToken(string token, string style)
{
    var client = new HttpClient();

    var disco = await cache.GetAsync();
    if (disco.IsError) throw new Exception(disco.Error);

    var request = new TokenExchangeTokenRequest
    {
        Address = disco.TokenEndpoint,
        ClientId = SampleConstants.Client_WeatherClientId,
        ClientSecret = SampleConstants.Client_WeatherClientSecret,

        SubjectToken = token,
        SubjectTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken,
        Scope = SampleConstants.GeographyScope,

        Parameters =
        {
            { "exchange_style", style }
        }
    };

    if (string.Equals(style, "delegation", StringComparison.OrdinalIgnoreCase))
    {
        var actorToken = await RequestTokenAsync(SampleConstants.Client_WeatherClientId,
            SampleConstants.Client_WeatherClientSecret, SampleConstants.GeographyScope);
        request.ActorToken = actorToken.AccessToken;
        request.ActorTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken;
    }

    var response = await client.RequestTokenExchangeTokenAsync(request);

    if (response.IsError)
    {
        throw new Exception(response.Error);
    }

    return response;
}