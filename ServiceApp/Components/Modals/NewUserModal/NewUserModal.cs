using Blazorise;
using Microsoft.AspNetCore.Components;
using ServiceApp.Models.DTO;
using ServiceApp.Shared;

namespace ServiceApp.Components.Modals.NewUserModal
{
    public partial class NewUserModal : ComponentBase
    {
        [Inject] private IUserService _userService { get; set; }
        [CascadingParameter] public MainLayout _mainLayout { get; set; }

        private bool _createCustomer { get; set; }

        private RolesComponent _rolesComponent;

        private Modal _modalRef;

        private UserRegisterDto _userRegisterDto = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private Task ShowModal(bool CreateCustomer)
        {
            this._createCustomer = CreateCustomer;
            return _modalRef.Show();
        }

        private Task HideModal()
        {
            return _modalRef.Hide();
        }

        private async void Save()
        {
            if (!_createCustomer && _rolesComponent._selectedRole is not null)
            {
                var role = _rolesComponent._roles.FirstOrDefault(x => x.RoleName == _rolesComponent._selectedRole);

                _userRegisterDto.RoleId = role.Id;

                var _ = await _userService.RegisterUser(_userRegisterDto);
                
            }

            CustomerDto customerDto = new()
            {
                Email = _userRegisterDto.Email,
                FirstName = _userRegisterDto.FirstName,
                LastName = _userRegisterDto.LastName,
                PhoneNumber = _userRegisterDto.PhoneNumber,
                IsActive = false
            };
            var customer = await _userService.CreateCustomer(customerDto);
            if (customer is null)
            {
                _mainLayout.alertComponent.ShowComponent("CustomerAlreadyExist",Color.Danger);
            }
            await HideModal();
        }


    }
}

