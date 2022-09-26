using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using ServiceApp.Models.Entities;
using ServiceApp.Tools;

namespace ServiceApp.Pages.Tickets
{
    public partial class Tickets : ComponentBase
    {
        [Inject] ITicketService? _ticketService { get; set; }
        [Inject] private PageHistoryState _pageHistoryState { get; set; }
        [Inject] public NavigationManager? _navigationManager { get; set; }

        public IEnumerable<Ticket> _ticketList { get; set; }

        private Ticket? _selectedTicket;

        protected override async Task OnInitializedAsync()
        {
            _pageHistoryState.AddPageToHistory(_navigationManager.Uri);
            _ticketList = await _ticketService.GetTickets();
            await base.OnInitializedAsync();
        }

        private async Task OnSelectedTicket()
        {
            if (_selectedTicket is not null)
            {
                _navigationManager.NavigateTo(PageDictionary.TicketDetailsPage(_selectedTicket.Id.ToString()));
            }

        }
    }
}
