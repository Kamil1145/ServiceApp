using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ServiceApp.ApiClient;
using ServiceApp.Models.DTO;
using ServiceApp.Tools;

namespace ServiceApp.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IApiClient _applicationClient;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;

        public TokenService(IApiClient applicationClient,
            ILocalStorageService localStorage,
            NavigationManager navigationManager)
        {
            _applicationClient = applicationClient;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
        }

        public async Task<string> GetNewAccessToken()
        {
            var tokensDto = await GetTokens();
            string newAccessToken = null;
            if (tokensDto is not null)
            {
                try
                {
                    newAccessToken = await _applicationClient.RefreshAccessTokenAsync(tokensDto);
                }
                catch (Exception e)
                {
                    await RemoveTokens();
                    _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage());
                }
            }

            if (!string.IsNullOrEmpty(newAccessToken))
            {
                await _localStorage.SetItemAsStringAsync("token", newAccessToken);
            }
            return newAccessToken;
        }

        public async Task<TokensResponseDto> GetTokens()
        {
            string token = await _localStorage.GetItemAsStringAsync("token");
            string refreshToken = await _localStorage.GetItemAsStringAsync("refresh_token");

            if (token is null || refreshToken is null)
            {
                return null;
            }
            var tokens = new TokensResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };

            return tokens;
        }

        public async Task RemoveTokens()
        {
            await _localStorage.RemoveItemAsync("token");
            await _localStorage.RemoveItemAsync("refresh_token");
        }
    }
}