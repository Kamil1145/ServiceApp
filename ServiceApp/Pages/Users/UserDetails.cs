using Microsoft.AspNetCore.Components;
using ServiceApp.Models.DTO;
using ServiceApp.Tools;

namespace ServiceApp.Pages.Users
{
    public partial class UserDetails : ComponentBase
    {
        [Inject] private PageHistoryState _pageHistoryState { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] public IUserService? _userService { get; set; }

        [Parameter] public string? _id { get; set; }

        public UserDto _user { get; set; } = new UserDto();


        protected override async Task OnInitializedAsync()
        {
            _pageHistoryState.AddPageToHistory(_navigationManager.Uri);
            _user = await _userService.GetUser(Guid.Parse(_id));
        }

        void Back()
        {
            _navigationManager.NavigateTo(_pageHistoryState.GetGoBackPage());
        }        
        
        void GoToUserEdit()
        {
            _navigationManager.NavigateTo(PageDictionary.UserEditPage(_user.Id.ToString()));
        }        
        
        void DeleteUser()
        {
            // remove user
        }
    }
}