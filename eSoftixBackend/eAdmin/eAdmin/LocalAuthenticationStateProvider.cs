using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using NETCore.Encrypt;
using System.Text.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using eModels;
namespace eAdmin
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {


        private readonly ILocalStorageService _storageService;
        public LocalAuthenticationStateProvider(ILocalStorageService storageService)
        {
            _storageService = storageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (await _storageService.ContainKeyAsync("_Authorization"))
                {
                    string str = await _storageService.GetItemAsync<string>("_Authorization");
                    var userInfo = JsonSerializer.Deserialize<UserModel>(EncryptProvider.Base64Decrypt(str));

                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name,userInfo.username),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.id.ToString()),
                    new Claim(ClaimTypes.Role,userInfo.role?.role_name),
                };

                    var identity = new ClaimsIdentity(claims, "BearerToken");
                    var user = new ClaimsPrincipal(identity);
                    var state = new AuthenticationState(user);
                    NotifyAuthenticationStateChanged(Task.FromResult(state));
                    return state;
                }

                return new AuthenticationState(new ClaimsPrincipal());
            }
            catch (System.Exception)
            {
                await _storageService.RemoveItemAsync("_Authorization");
                return new AuthenticationState(new ClaimsPrincipal());

                throw;
            }
            
        }

        public async Task LogoutAsync()
        {
            await _storageService.RemoveItemAsync("_Authorization");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }

}
