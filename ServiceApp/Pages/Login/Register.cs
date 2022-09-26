using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceApp.Models.DTO;
using ServiceApp.Tools;

namespace ServiceApp.Pages.Login;
partial class Register
{
    [Inject] public ILocalStorageService _localStorage { get; set; }

    [Inject] public AuthenticationStateProvider _authenticationStateProvider { get; set; }

    [Inject] public NavigationManager? _navigationManager { get; set; }

    [Inject] IUserService? _userService { get; set; }

    UserRegisterDto _user = new();

    async Task HandleRegister()
    {
        var result = await _userService.RegisterUser(_user);
        if (result!= null)
        {
            _navigationManager.NavigateTo(PageDictionary.RegisteredPage());
        }
    }
}
