using Microsoft.AspNetCore.Components;
using Blazorise;
using ServiceApp.Tools;
using ServiceApp.Models.DTO;
namespace ServiceApp.Pages.Tickets;

public partial class NewTicket
{
    [Inject] private ITicketService _ticketService { get; set; }

    [Inject] private IUserService _userService { get; set; }

    [Inject] private NavigationManager _navigationManager { get; set; }

    private TicketDto _ticketDto = new();
    private UserDto _userContext { get; set; }

    protected override async void OnInitialized()
    {
        base.OnInitialized();
        _userContext = await _userService.GetUserContext();
        StateHasChanged();
    }

    private async void Save()
    {
        CreateTicketDto ticket = new()
        {Title = _ticketDto.Title, Description = _ticketDto.Description };
        await _ticketService.CreateTicket(ticket);
        GoToSendTicketPage();
    }

    private async void GoToSendTicketPage()
    {
        _navigationManager.NavigateTo(PageDictionary.SentTicketPage());
    }

    void ValidateTicketTitle(ValidatorEventArgs e)
    {
        var title = Convert.ToString(e.Value);
        e.Status = string.IsNullOrEmpty(title) ? ValidationStatus.None : title.Length >= (10) ? ValidationStatus.Success : ValidationStatus.Error;
    }

    void ValidateTicketDescription(ValidatorEventArgs e)
    {
        var description = Convert.ToString(e.Value);
        e.Status = string.IsNullOrEmpty(description) ? ValidationStatus.None : description.Length >= (20) ? ValidationStatus.Success : ValidationStatus.Error;
    }
}