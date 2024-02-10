using L3T.Infrastructure.Helpers.DataContext;
using L3TIdentityTokenServer.DataAccess;

namespace L3TIdentityTokenServer.Extention
{
    public static class OpenIddictExtention
    {
        public static IServiceCollection AddopenIddictExtention(this IServiceCollection services)
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
                    // Enable the token endpoint.
                    options.SetTokenEndpointUris("/api/connect/token")
                        .SetAuthorizationEndpointUris("/api/connect/authorize")
                        .SetIntrospectionEndpointUris("/api/connect/introspect");

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
                        .EnableTokenEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .EnableStatusCodePagesIntegration();
                    options.SetAccessTokenLifetime(TimeSpan.FromDays(1));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(14));
                })
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });
            
            services.AddHostedService<TestData>();

            return services;
        }
    }
}