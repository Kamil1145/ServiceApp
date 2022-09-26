using Microsoft.AspNetCore.Components;

namespace ServiceApp.Pages.Login
{
    public partial class ActivateAccount
    {
        [Inject] IUserService _userService { get; set; }

        [Parameter] public string _id { get; set; }

        public string _text { get; set; } = string.Empty;
        
        protected override async void OnInitialized()
        {
            _text = await _userService.ActivateUser(_id);
            StateHasChanged();
        }
    }
}