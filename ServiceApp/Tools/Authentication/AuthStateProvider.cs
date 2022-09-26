using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceApp.API.Exceptions;
using ServiceApp.ApiClient;

namespace ServiceApp.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IApiClientBase _apiClientBase;
        private readonly ITokenService _tokenService;

        public AuthStateProvider(
            ILocalStorageService localStorage,
            IApiClientBase apiClientBase,
            ITokenService tokenService)
        {
            _localStorage = localStorage;
            _apiClientBase = apiClientBase;
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _localStorage.GetItemAsStringAsync("token");
            string refreshToken = await _localStorage.GetItemAsStringAsync("refresh_token");

            var identity = new ClaimsIdentity();
            _apiClientBase.httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(refreshToken))
            {
                var claims = ParseClaimsFromJwt(token);
                if (claims != null && CheckTokenExpiration(claims.FirstOrDefault(x => x.Type == "exp").Value)
                    && !string.IsNullOrEmpty(refreshToken))
                {
                    var _ = await _tokenService.GetNewAccessToken();
                }
                identity = new ClaimsIdentity(claims, "jwt");
                _apiClientBase.httpClient.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        public bool CheckTokenExpiration(string value)
        {
            long expires = long.Parse(value);
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expires).LocalDateTime;
            var now = DateTime.Now;
            return (expirationDate - now).TotalMinutes <= 10;
        }

        public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var roles = GetRolesFromJwtFromArray(keyValuePairs);

            if (roles is null)
            {
                _tokenService.RemoveTokens();
                throw new CredentialsException("User does not have any role assinged");
            }

            return roles.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()))
                .ToList();
        }


        private List<KeyValuePair<string, object>> GetRolesFromJwtFromArray(Dictionary<string, object> deserializedClaims)
        {
            List<KeyValuePair<string, object>> claimList = null;
            if (deserializedClaims.TryGetValue(ClaimTypes.Role, out var roleClaim))
            {
                deserializedClaims.Remove(ClaimTypes.Role);

                var roles = roleClaim.ToString()
                    .Split(',')
                    .ToArray()
                    .Select(a => ParseToLetterString(a))
                    .ToArray();

                claimList = deserializedClaims.ToList();

                foreach (var role in roles)
                {
                    claimList.Add(new KeyValuePair<string, object>(ClaimTypes.Role, role));
                }
            }

            return claimList;
        }



        private string ParseToLetterString(string text)
        {
            string value = string.Empty;
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                    value += c;
            }

            return value;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
