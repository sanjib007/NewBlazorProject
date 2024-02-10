using Microsoft.Extensions.DependencyInjection;

namespace L3T.PPPoEService.Extention;

public static class OpeniddictExtention
{
    public static IServiceCollection AddOpeniddictExtention(this IServiceCollection services,
        string url, string clientId, string secret)
    {
        services.AddOpenIddict()
            .AddValidation(options =>
            {
                // Note: the validation handler uses OpenID Connect discovery
                // to retrieve the address of the introspection endpoint.
                options.SetIssuer(url);
                //options.SetIssuer("https://localhost:7008/");
                //options.AddAudiences("postman3");

                // Configure the validation handler to use introspection and register the client
                // credentials used when communicating with the remote introspection endpoint.
                options.UseIntrospection()
                    .SetClientId(clientId)
                    //.SetClientId("cams")
                    .SetClientSecret(secret);
                //.SetClientSecret("test123");

                // Register the System.Net.Http integration.
                options.UseSystemNetHttp();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });
        return services;
    }
}