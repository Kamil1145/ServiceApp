using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Rest;
using ServiceApp.ApiClient;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using ServiceApp.Tools;

namespace ServiceApp.Services
{
    public class UserService : IUserService
    {
        private readonly IApiClient _applicationClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserService(
            IApiClient client,
            ILocalStorageService localStorageService,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _applicationClient = client;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> ActivateUser(string id)
        {
            return await _applicationClient.ActivateAccountAsync(Guid.Parse(id));
        }

        public async Task<Customer> CreateCustomer(CustomerDto userDto)
        {
            try
            {
                return await _applicationClient.CreateCustomerAsync(userDto);
            }
            catch (ApiException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<UserDto> GetUser(Guid id)
        {
            return await _applicationClient.GetUserAsync(id);
        }

        public async Task<UserDto> GetUserContext()
        {
            var userContext = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;

            if (userContext.Claims.Any())
            {
                return await _applicationClient.GetUserByEmailAsync(userContext.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value);
            }
            return null;
        }

        public async Task<IEnumerable<UserDto>?> GetUsers()
        {
            IEnumerable<UserDto> users = null;
            try
            {
                users = (await _applicationClient.GetUsersAsync().ConfigureAwait(false)).Take(25);
            }
            catch (ApiException e)
            {
                _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage());
            }

            return users;
        }
        public async Task<IEnumerable<UserDto>?> GetEmployees()
        {
            ICollection<UserDto> users = null;
            try
            {
                users = await _applicationClient.GetEmployeesAsync();
            }
            catch (ApiException e)
            {
                _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage());
            }

            return users;
        }

        public async Task<IEnumerable<CustomerDto>?> GetCustomers()
        {
            ICollection<CustomerDto> users = null;
            try
            {
                users = await _applicationClient.GetAllCustomersAsync();
            }
            catch (ApiException e)
            {
                _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage());
            }

            return users;
        }

        public async Task<string?> LoginUser(UserLoginDto userDto)
        {
            TokensResponseDto tokens = null;
            try
            {
                tokens = await _applicationClient.LoginAsync(userDto);
            }
            catch (ApiException)
            {
                _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage());
            }

            if (tokens is not null)
            {
                await _localStorageService.SetItemAsStringAsync("token", tokens.AccessToken);
                await _localStorageService.SetItemAsStringAsync("refresh_token", tokens.RefreshToken);
                return tokens.AccessToken;
            }
            return null;
        }

        public async Task<UserLoginDto> RegisterUser(UserRegisterDto userRegisterDto)
        {
            UserLoginDto user = null;
            try
            {
                user = await _applicationClient.RegisterUserAsync(userRegisterDto);
            }
            catch (ApiException e)
            {
                _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage(e.Message));
            }
            return user;
        }

        public async Task<string> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            return await _applicationClient.ResetPasswordAsync(resetPasswordDto);
        }

        public async Task<User> UpdateUser(UserDto userDto)
        {
            var result = await _applicationClient.UpdateUserAsync(userDto);
            return result;

        }
    }
}