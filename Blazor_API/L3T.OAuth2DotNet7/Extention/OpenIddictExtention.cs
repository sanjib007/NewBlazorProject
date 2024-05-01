using L3T.OAuth2DotNet7.DataAccess;
using Microsoft.Extensions.Options;

namespace L3T.OAuth2DotNet7.Extention
{
    public static class OpenIddictExtention
    {
        public static IServiceCollection AddopenIddictExtention(this IServiceCollection services, ConfigurationManager Configuration)
        {
            services.AddCors();
            services.AddOpenIddict()

                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and models.
                    // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                    options.UseEntityFrameworkCore()
                        .UseDbContext<IdentityTokenServerDBContext>()
                        //.ReplaceDefaultEntities<long>()
                        ;

                })

                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    //Enable the token endpoint.
                    options.SetTokenEndpointUris(Configuration.GetValue<string>("CRSetAuthURL:TokenEndpointUris"))
                        .SetAuthorizationEndpointUris(Configuration.GetValue<string>("CRSetAuthURL:AuthorizationEndpointUris"))
                        .SetIntrospectionEndpointUris(Configuration.GetValue<string>("CRSetAuthURL:IntrospectionEndpointUris"))
                        .SetLogoutEndpointUris(Configuration.GetValue<string>("CRSetAuthURL:LogoutEndpointUris"));



                    // Enable the token endpoint.
                    //options.SetTokenEndpointUris("/CRIdentity/api/connect/token")
                    //    .SetAuthorizationEndpointUris("/CRIdentity/api/connect/authorize")
                    //    .SetIntrospectionEndpointUris("/connect/introspect")
                    //    .SetLogoutEndpointUris("/CRIdentity/api/connect/logout");

                    // Enable the password flow.
                    options.AllowAuthorizationCodeFlow()
                        .AllowClientCredentialsFlow()
                        .AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    // Accept anonymous clients (i.e clients that don't send a client_id).
                    //options.AcceptAnonymousClients();

                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .DisableTransportSecurityRequirement() //need to remove
                        .EnableStatusCodePagesIntegration();
                 
                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(Configuration.GetValue<int>("TokenTime:AccessTokenMin")));
                    options.SetRefreshTokenLifetime(TimeSpan.FromMinutes(Configuration.GetValue<int>("TokenTime:RefreshTokenMin")));
                })
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            //services.AddHostedService<TestData>();

            return services;
        }
    }
}