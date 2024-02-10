using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Cr.UI.Services.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Security.Claims;

namespace Cr.UI.Data
{
    public class CustomAuthenticationStateProvidore : AuthenticationStateProvider
    {
        public ILocalStorageService _localStorage;
        public IUserService _userService;
        public IMenuAndPermissionService _menuAndPermissionService;

        public CustomAuthenticationStateProvidore(
            ILocalStorageService localStorage, 
            IUserService userService,
            IMenuAndPermissionService menuAndPermissionService)
        {
            _localStorage = localStorage;
            _userService = userService;
            _menuAndPermissionService = menuAndPermissionService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var expireIn = await _localStorage.GetItemAsync<string>("expire_in");
            var subject = await _localStorage.GetItemAsync<string>("subject");
            var email = await _localStorage.GetItemAsync<string>("email");

            ClaimsIdentity identity;

            if (Convert.ToDateTime(expireIn) < DateTime.Now)
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    ApiResponse model = await _userService.RefreshTokenAsync();
                    UserModel user = (UserModel)model.Data;
                    if (user != null)
                    {
                        await setLocalStorage(user);
                        identity = GetClaimsIdentity(user);
                    }
                    else
                    {
                        identity = new ClaimsIdentity();
                    }
                }
                else
                {
                    identity = new ClaimsIdentity();
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    var userMod = new UserModel()
                    {
                        Subject = subject,
                        Email = email
                    };
                    identity = GetClaimsIdentity(userMod);
                }
                else
                {
                    identity = new ClaimsIdentity();
                }
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        private ClaimsIdentity GetClaimsIdentity(UserModel user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.Subject != null)
            {
                claimsIdentity = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name, user.Subject),
                                    new Claim(ClaimTypes.Email, user.Email)
                                }, "apiauth_type1");
            }

            return claimsIdentity;
        }

        public async Task MarkUserAsAuthenticated(UserModel user)
        {
            await setLocalStorage(user);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            _userService.Logout();
            await removeLocalStorage();            
        }

        private async Task<bool> setLocalStorage(UserModel user)
        {
            await _userService.setLocalStorage(user);
            var allPermissionResponse = await _menuAndPermissionService.GetAllMenuWithPermission("Account/GetAllMenuAndPermission");
            if (allPermissionResponse.Status == "Error" || allPermissionResponse.StatusCode == 400)
            {
                await _userService.removedLocalStorage();
                return false;
            }            
            return true;
        }
        private async Task removeLocalStorage()
        {
            await _userService.removedLocalStorage();
        }
    }


}
