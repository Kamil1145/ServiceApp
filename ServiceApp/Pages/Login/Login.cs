using Blazored.LocalStorage;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using ServiceApp.Models.DTO;
using ServiceApp.Shared;
using ServiceApp.Tools;
namespace ServiceApp.Pages.Login;

partial class Login
{
    [Inject] public IUserService _userService { get; set; }
    [Inject] public ILocalStorageService _localStorage { get; set; }
    [Inject] public AuthenticationStateProvider _authenticationStateProvider { get; set; }
    [Inject] public NavigationManager _navigationManager { get; set; }
    [CascadingParameter] public MainLayout _mainLayout { get; set; }

    private ErrorBoundary? errorBoundary;

    UserLoginDto user = new();

    async Task HandleLogin()
    {
        try
        {
            var token = await _userService.LoginUser(user);
            if (string.IsNullOrEmpty(token))
            {
                _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage());
            }
            else
            {
                await _authenticationStateProvider.GetAuthenticationStateAsync();
                _navigationManager.NavigateTo(PageDictionary.IndexPage());
            }
        }
        catch (Exception e)
        {
            _mainLayout.alertComponent.ShowComponent(e.Message, Color.Warning);

        }

    }
}

