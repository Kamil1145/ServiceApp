using Microsoft.AspNetCore.Components;
using ServiceApp.Shared;
using ServiceApp.Components;
using ServiceApp.Tools;
using ServiceApp.Models.DTO;
using Blazorise;

namespace ServiceApp.Pages.Users
{
    public partial class EditUser
    {
        #region DI
        [Inject] public IUserService? _userService { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] PageHistoryState _pageHistoryState { get; set; }
        #endregion

        #region PARAMETERS        
        [Parameter] public string? _id { get; set; }
        [CascadingParameter] public MainLayout _mainLayout { get; set; }
        #endregion

        public UserDto _user { get; set; } = new UserDto();

        RolesComponent _rolesComponent;

        protected override async Task OnParametersSetAsync()
        {
            _pageHistoryState.AddPageToHistory(_navigationManager.Uri);
            _user = await _userService.GetUser(Guid.Parse(_id));
        }
        private async void Save()
        {
            await _userService.UpdateUser(_user);
            _navigationManager.NavigateTo(PageDictionary.UserDetailsPage(_user.Id.ToString()));
        }

        private async void AddRoleToUser()
        {
            string? selectedRole = _rolesComponent._selectedRole;
            if (selectedRole is not null)
            {
                var role = _rolesComponent._roles.FirstOrDefault(x => x.RoleName == selectedRole);
                RoleDto newRoleDto = new RoleDto()
                { Id = role.Id, RoleName = role.RoleName };
                if (!_user.Roles.Any(x => x.RoleName == selectedRole))
                {
                    _user.Roles.Add(newRoleDto);
                }
                else
                {
                    await _mainLayout.alertComponent.ShowComponent("Role already assigned", Color.Warning);
                }
            }
        }

        private async void OnButtonClicked(int id)
        {
            var role = _user.Roles.FirstOrDefault(x => x.Id == id);
            _user.Roles.Remove(role);
        }

        void Back()
        {
            _navigationManager.NavigateTo(_pageHistoryState.GetGoBackPage());
        }
    }
}