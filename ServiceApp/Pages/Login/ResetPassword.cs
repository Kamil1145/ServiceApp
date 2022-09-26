using Blazorise;
using Microsoft.AspNetCore.Components;
using ServiceApp.ApiClient;
using ServiceApp.Models.DTO;
using ServiceApp.Shared;

namespace ServiceApp.Pages.Login
{
    public partial class ResetPassword
    {
        [Inject] IUserService _userService { get; set; }

        [Parameter] public string _id { get; set; }

        [CascadingParameter] public MainLayout _mainLayout { get; set; }

        public ResetPasswordDto _resetPasswordDto { get; set; } = new();

        protected override void OnInitialized()
        {
            _resetPasswordDto.Token = _id;
            base.OnInitialized();
        }

        public async Task Confirm()
        {
            if (_resetPasswordDto.Password == _resetPasswordDto.ConfirmPassword)
            {
                try
                {
                    var text  = await _userService.ResetPassword(_resetPasswordDto);
                    await _mainLayout.alertComponent.ShowComponent(text, Color.Success);
                }
                catch (ApiException)
                {
                    await _mainLayout.alertComponent.ShowComponent("Error during password setting", Color.Danger);
                }
            }
            else
            {
                await _mainLayout.alertComponent.ShowComponent("Passwords doesn't match", Color.Danger);
            }
        }
    }
}