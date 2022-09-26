using Blazorise;
using Microsoft.AspNetCore.Components;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

namespace ServiceApp.Components.Modals.TicketModal
{
    public partial class TicketModal : ComponentBase
    {
        [Inject] public ITicketService _ticketService { get; set; }
        [Parameter] public TicketDto _ticketDto { get; set; }

        public Modal _modalRef;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private Task ShowModal()
        {
            return _modalRef.Show();
        }

        private Task HideModal()
        {
            return _modalRef.Hide();
        }

        private async void Save()
        {
            await _ticketService.UpdateTicket(_ticketDto);
            HideModal();
        }
    }
}

