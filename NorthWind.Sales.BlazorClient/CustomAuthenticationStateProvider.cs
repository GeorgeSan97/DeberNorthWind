using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using NorthWind.Membership.Entities.UserLogin;

namespace NorthWind.Sales.BlazorClient
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        private System.Timers.Timer _refreshTimer;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            
            // Configurar un timer para verificar el estado de autenticación cada 2 segundos
            _refreshTimer = new System.Timers.Timer(2000);
            _refreshTimer.Elapsed += async (sender, e) => await RefreshAuthenticationState();
            _refreshTimer.Start();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Intentar obtener los tokens del sessionStorage con la clave que usa el Membership
                var tokensJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "tokens");
                
                if (!string.IsNullOrEmpty(tokensJson))
                {
                    // Deserializar el objeto TokensDto
                    var tokens = JsonSerializer.Deserialize<TokensDto>(tokensJson);
                    
                    if (tokens != null && !string.IsNullOrEmpty(tokens.AccessToken))
                    {
                        // Parsear el token JWT para obtener los claims
                        var claims = ParseJwtToken(tokens.AccessToken);
                        if (claims.Count > 0)
                        {
                            _currentUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
                            return new AuthenticationState(_currentUser);
                        }
                    }
                }
            }
            catch
            {
                // Si hay error, retornar usuario no autenticado
            }

            // Retornar usuario no autenticado por defecto
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(_currentUser);
        }

        private List<Claim> ParseJwtToken(string token)
        {
            var claims = new List<Claim>();
            
            try
            {
                // Simple JWT parsing - obtener el payload
                var parts = token.Split('.');
                if (parts.Length > 1)
                {
                    var payload = parts[1];
                    // Ajustar padding para base64
                    payload = payload.Replace('-', '+').Replace('_', '/');
                    switch (payload.Length % 4)
                    {
                        case 2: payload += "=="; break;
                        case 3: payload += "="; break;
                    }
                    
                    var jsonBytes = System.Convert.FromBase64String(payload);
                    var json = System.Text.Encoding.UTF8.GetString(jsonBytes);
                    var tokenData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                    
                    if (tokenData != null)
                    {
                        foreach (var claim in tokenData)
                        {
                            if (claim.Value is JsonElement element)
                            {
                                var value = element.ValueKind == JsonValueKind.String 
                                    ? element.GetString() 
                                    : element.GetRawText();
                                claims.Add(new Claim(claim.Key, value));
                            }
                            else if (claim.Value != null)
                            {
                                claims.Add(new Claim(claim.Key, claim.Value.ToString()));
                            }
                        }
                    }
                }
            }
            catch
            {
                // Si hay error parsing, retornar claims vacíos
            }
            
            return claims;
        }

        public void MarkUserAsAuthenticated(ClaimsPrincipal user)
        {
            _currentUser = user;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void MarkUserAsLoggedOut()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // Método para forzar la recarga del estado de autenticación
        public async Task RefreshAuthenticationState()
        {
            // Limpiar cualquier token residual de localStorage por si acaso
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "tokens");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Dispose()
        {
            _refreshTimer?.Dispose();
        }
    }
}
