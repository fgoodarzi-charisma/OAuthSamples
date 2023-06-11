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

                options.ClientId = "web";
                options.ClientSecret = "secret";

                options.ResponseType = "code";
                options.UsePkce = true;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("weather");

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
            });

        // add automatic token management
        services.AddAccessTokenManagement();

        // add HTTP client to call protected API
        services.AddUserAccessTokenHttpClient("smpl__weather_api", configureClient: client =>
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