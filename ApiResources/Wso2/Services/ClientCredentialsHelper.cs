using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wso2.Services;

public class ClientCredentialsHelper
{
    public static async Task<IdentityModel.Client.TokenResponse> GetToken(string username, string password)
    {
        var cache = new DiscoveryCache("https://localhost:5001");
        var disco = await cache.GetAsync();

        var actorRequest = new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = username,
            ClientSecret = password,
            Scope = "geography",
        };

        var client = new HttpClient();

        return await client.RequestClientCredentialsTokenAsync(actorRequest);
    }
}