using Microsoft.AspNetCore.Components;
using ServiceApp.Tools;

namespace ServiceApp.Pages
{
    public partial class Index
    {
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private PageHistoryState _pageHistoryState { get; set; }
        private bool _isInitialized = false;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            _pageHistoryState.AddPageToHistory(_navigationManager.Uri);
            _isInitialized = true;
        }

        public void GoToUsersList()
        {
            _navigationManager.NavigateTo(PageDictionary.UserListPage());
        }

        public void GoToTicketsList()
        {
            _navigationManager.NavigateTo(PageDictionary.TicketsPage());
        }

        public void GoToNewTicket()
        {
            _navigationManager.NavigateTo(PageDictionary.FormPage());
        }

        public void GoToClientList()
        {
            _navigationManager.NavigateTo(PageDictionary.CustomersPage());
        }
    }
}