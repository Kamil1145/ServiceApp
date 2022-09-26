using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using ServiceApp.Models.DTO;
using ServiceApp.Tools;

namespace ServiceApp.Pages.Users
{
    partial class UserList : ComponentBase
    {
        [Inject] private PageHistoryState _pageHistoryState { get; set; }
        [Inject] public NavigationManager? _navigationManager { get; set; }
        [Inject] public IUserService? _userService { get; set; }

        private UserDto? _selectedUser;

        public IEnumerable<UserDto>? _users { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _pageHistoryState.AddPageToHistory(_navigationManager.Uri);
            _users = await _userService.GetUsers();
        }
        
        private async Task OnSelectedUser()
        {
            if (_selectedUser is not null)
            {
                _navigationManager.NavigateTo(PageDictionary.UserDetailsPage(_selectedUser.Id.ToString()));
            }
        }
    }
}