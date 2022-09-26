using Blazorise;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using ServiceApp.Components;
using ServiceApp.Components.Modals.TicketModal;
using ServiceApp.Components.Modals.UsersModal;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using ServiceApp.Shared;
using ServiceApp.Tools;

namespace ServiceApp.Pages.Tickets
{
    public partial class TicketPage
    {
        [Inject] public ITicketService _ticketService { get; set; }
        [Inject] public IUserService _userService { get; set; }
        [Inject] private PageHistoryState _pageHistoryState { get; set; }
        [Inject] public NavigationManager? _navigationManager { get; set; }
        [Inject] IMessageService _messageService { get; set; }
        [Parameter] public string _id { get; set; }
        [CascadingParameter] public MainLayout _mainLayout { get; set; }

        private TicketStatusComponent _ticketStatusComponent { get; set; }
        private TicketPriorityComponent _ticketPriorityComponent { get; set; }
        private UsersModal _userModal { get; set; }
        private TicketModal _ticketModalref { get; set; }
        private UserDto _userContext { get; set; }

        private bool _showComment;
        private string _buttonText = "Add Comment";

        private string _ticketComment = string.Empty;

        public TicketDto TicketDto { get; set; } = new TicketDto()
        {
            ResponsibleUser = new UserDto()
        };


        protected override async Task OnInitializedAsync()
        {
            _pageHistoryState.AddPageToHistory(_navigationManager.Uri);
            _userContext = await _userService.GetUserContext();
            LoadTicket();
            StateHasChanged();
        }

        public async void LoadTicket()
        {
            TicketDto = await _ticketService.GetTicket(Guid.Parse(_id));
            if (_ticketStatusComponent != null && _ticketPriorityComponent != null)
            {
                _ticketStatusComponent._selectedStatus = TicketDto.TicketStatus.ToString();
                _ticketPriorityComponent._selectedStatus = TicketDto.Priority.ToString();
            }

            StateHasChanged();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_ticketModalref != null)
            {
                _ticketModalref._ticketDto = TicketDto;
            }
            base.OnAfterRender(firstRender);
        }

        private async void SaveComment()
        {
            if (!_ticketComment.IsNullOrEmpty())
            {
                await _ticketService.AddCommentToTicket(_id, _ticketComment);
            }
            SwitchVisibilityCommentTexbox();
            LoadTicket();
            _ticketComment = string.Empty;
        }

        private async void SaveTicketStatus()
        {
            if (_ticketStatusComponent != null && _ticketPriorityComponent != null)
            {
                OverrideTicketEnums();
            }

            if (_userModal._selectedUser != null)
            {
                TicketDto.ResponsibleUser = _userModal._selectedUser;
            }
            await _ticketService.UpdateTicket(TicketDto);
        }

        private void OverrideTicketEnums()
        {
            Enum.TryParse(_ticketStatusComponent._selectedStatus, out TicketStatus statusEnum);
            TicketDto.TicketStatus = statusEnum;

            Enum.TryParse(_ticketPriorityComponent._selectedStatus, out TicketPriority ticketPriority);
            TicketDto.Priority = ticketPriority;
        }


        public void SwitchVisibilityCommentTexbox()
        {
            _showComment = !_showComment;

            if (!_showComment)
            {
                _buttonText = "Add Comment";
            }
            else
            {
                _buttonText = "Hide comment editor";
            }
            StateHasChanged();
        }

        public async void DeleteTicket()
        {
            if (await _messageService.Confirm("Do you want to delete ticket?", "Confirmation"))
            {
                var result = await _ticketService.DeleteTicket(_id);
                if (result)
                {
                    await _mainLayout.alertComponent.ShowComponent($"Ticket deleted, redirecting...", Color.Success, spinner: true);
                    _navigationManager.NavigateTo(PageDictionary.TicketsPage());
                }
            }
        }

        public async void CreateJiraIssueAction()
        {
            var jiraTicketExist = !string.IsNullOrWhiteSpace(TicketDto.JiraTicketId);
            if (jiraTicketExist)
            {
                if (await _messageService.Confirm("This ticket has JiraID. Are you sure you want to create new Jira ticket?", "Confirmation"))
                {
                    CreateJiraTicket();
                }
            }
            else
            {
                CreateJiraTicket();
            }
        }

        private async void CreateJiraTicket()
        {
            var key = await _ticketService.CreateJiraIssue(Guid.Parse(_id));
            if (!key.IsNullOrEmpty())
                _mainLayout.alertComponent.ShowComponent($"Ticket created with id: {key}", Color.Success);
            else
                _mainLayout.alertComponent.ShowComponent("ERROR", Color.Danger);

        }
    }
}
