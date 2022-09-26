using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using ServiceApp.Models.DTO;
using ServiceApp.Tools;

namespace ServiceApp.Pages.Customers;

public partial class Customers
{

    [Inject] public IUserService? _userService { get; set; }

    [Inject] public NavigationManager? _navigationManager { get; set; }

    private CustomerDto? _selectedUser;

    public IEnumerable<CustomerDto>? _customers { get; set; } = new List<CustomerDto>();


    private int _totalUsers;

    protected override async Task OnInitializedAsync()
    {
        _customers = await _userService.GetCustomers();
    }

    private async Task OnSelectedUser()
    {
        if (_selectedUser is not null)
        {
            _navigationManager.NavigateTo(PageDictionary.UserDetailsPage(_selectedUser.Id.ToString()));
        }

    }
}
