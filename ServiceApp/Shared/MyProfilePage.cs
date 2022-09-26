using Microsoft.AspNetCore.Components;
using ServiceApp.Models.DTO;
using ServiceApp.Tools;

namespace ServiceApp.Shared
{
    public partial class MyProfilePage
    {
        [Inject] private PageHistoryState _pageHistoryState { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private IUserService _userService { get; set; }

        private UserDto _user { get; set; } = null;

        protected override async void OnInitialized()
        {
            _pageHistoryState.AddPageToHistory(_navigationManager.Uri);
            _user = await _userService.GetUserContext();
            StateHasChanged();
        }
    }
}