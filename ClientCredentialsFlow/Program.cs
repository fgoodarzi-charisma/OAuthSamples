using IdentityModel.Client;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

// discover endpoints from metadata
var handler = new SocketsHttpHandler();
var path = $"{Environment.CurrentDirectory}/cert-11.pfx";
var cert = new X509Certificate2(path, "fF@123456");
handler.SslOptions.ClientCertificates = new X509CertificateCollection { cert };

var client = new HttpClient(handler)
{
    BaseAddress = new Uri("https://localhost:5001"),
};

var disco = await client.GetDiscoveryDocumentAsync();
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.MtlsEndpointAliases.TokenEndpoint,
    ClientId = "client-credentials",
    Scope = "identity",
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine(tokenResponse.ErrorDescription);
    return;
}

// call api
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

var response = await apiClient.GetAsync("https://localhost:6001/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var content = await response.Content.ReadAsStringAsync();

    var parsed = JsonDocument.Parse(content);
    var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

    Console.WriteLine(formatted);
}
