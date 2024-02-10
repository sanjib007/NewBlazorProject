using L3T.OAuth2DotNet7.DataAccess;
using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace L3T.OAuth2DotNet7
{
    public class TestData : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public TestData(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityTokenServerDBContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("Test", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "Test",
                    ClientSecret = "test123",
                    DisplayName = "Test",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Endpoints.Introspection
                        

                        //OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                    }
                }, cancellationToken);
            }
            if (await manager.FindByClientIdAsync("Client1", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "Client1",
                    ClientSecret = "client123",
                    DisplayName = "Client1",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Introspection
                    }
                }, cancellationToken);
            }
            if (await manager.FindByClientIdAsync("Client2", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "Client2",
                    ClientSecret = "client123",
                    DisplayName = "Client2",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Introspection
                    }
                }, cancellationToken);
            }

            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRoles>>();

            var loguser = _userManager.FindByEmailAsync("admin@example.com");
            if (loguser.Result == null)
            {
                var isHave = await _roleManager.RoleExistsAsync("Admin");
                if (!isHave)
                {
                    var newRole = new AppRoles()
                    {
                        Name = "Admin"
                    };
                    await _roleManager.CreateAsync(newRole);
                }
                var user1 = new AppUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FullName = "System",
                    PhoneNumber = "SystemL3T",
                    Userid = "System",
                    User_designation = "System",
                    Department = "System",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "system",
                    IsActive = true
                };
                var password = "L3T123..";
                await _userManager.CreateAsync(user1, password);
                await _userManager.AddToRoleAsync(user1, "Admin");

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}