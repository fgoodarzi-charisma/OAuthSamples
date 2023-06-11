using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace MvcCodeFlow;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        services.AddControllersWithViews();
        services.AddHttpClient();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "oidc";
        })
            .AddCookie(options =>
            {
                options.Cookie.Name = "mvc";

                options.Events.OnSigningOut = async e =>
                {
                    await e.HttpContext.RevokeUserRefreshTokenAsync();
                };
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = SampleConstants.StsBaseUrl;
                options.RequireHttpsMetadata = false;

                options.ClientId = SampleConstants.Client_CodeId;
                options.ClientSecret = SampleConstants.Client_CodeSecret;

                options.ResponseType = OidcConstants.ResponseTypes.Code;
                options.UsePkce = true;

                options.Scope.Clear();
                options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                options.Scope.Add(OidcConstants.StandardScopes.Profile);
                options.Scope.Add(SampleConstants.WeatherScope);

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
            });

        // add automatic token management
        services.AddAccessTokenManagement();

        // add HTTP client to call protected API
        services.AddUserAccessTokenHttpClient(SampleConstants.Api_WeatherId, configureClient: client =>
        {
            client.BaseAddress = new Uri(SampleConstants.WeatheApiBaseUrl);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute()
                .RequireAuthorization();
        });
    }
}