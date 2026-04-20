using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace Youg_Inventory_System.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _protectedSessionStorage;
        private readonly ILogger<CustomAuthenticationStateProvider> _logger;

        public CustomAuthenticationStateProvider(ProtectedSessionStorage protectedSessionStorage, 
            ILogger<CustomAuthenticationStateProvider> logger)
        {
            _protectedSessionStorage = protectedSessionStorage;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await _protectedSessionStorage.GetAsync<string>("auth-token");
                var username = result.Success ? result.Value : null;

                if (string.IsNullOrEmpty(username))
                {
                    // User is not authenticated - return anonymous principal
                    var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
                    return new AuthenticationState(anonymousPrincipal);
                }

                // User is authenticated - return authenticated principal with claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("user.name", username)
                };

                var identity = new ClaimsIdentity(claims, "auth");
                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            }
            catch (InvalidOperationException)
            {
                // Occurs during prerendering or when JavaScript is unavailable - fail silently
                var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymousPrincipal);
            }
            catch (JSDisconnectedException)
            {
                // Occurs when JavaScript runtime disconnects - fail silently
                var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymousPrincipal);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected exceptions
                _logger.LogError(ex, "Error retrieving authentication state");
                var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymousPrincipal);
            }
        }

        public async Task MarkUserAsAuthenticated(string username)
        {
            try
            {
                await _protectedSessionStorage.SetAsync("auth-token", username);

                // Create authenticated principal
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("user.name", username)
                };

                var identity = new ClaimsIdentity(claims, "auth");
                var principal = new ClaimsPrincipal(identity);

                // Notify that authentication state has changed
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
            }
            catch (InvalidOperationException)
            {
                // Occurs during prerendering or when JavaScript is unavailable - fail silently
            }
            catch (JSDisconnectedException)
            {
                // Occurs when JavaScript runtime disconnects - fail silently
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking user as authenticated");
            }
        }

        public async Task MarkUserAsLoggedOut()
        {
            try
            {
                await _protectedSessionStorage.DeleteAsync("auth-token");

                // Create anonymous principal
                var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

                // Notify that authentication state has changed
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousPrincipal)));
            }
            catch (InvalidOperationException)
            {
                // Occurs during prerendering or when JavaScript is unavailable - fail silently
            }
            catch (JSDisconnectedException)
            {
                // Occurs when JavaScript runtime disconnects - fail silently
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking user as logged out");
            }
        }
    }
}
