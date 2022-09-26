using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceApp.ApiClient;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using ServiceApp.Tools;

namespace ServiceApp.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IApiClient _applicationClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public TicketService(IApiClient applicationClient,
            AuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager)
        {
            _applicationClient = applicationClient;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            ICollection<Ticket> tickets = null;
            try
            {
                tickets = await _applicationClient.GetTicketsAsync();
            }
            catch (ApiException e)
            {
                _navigationManager.NavigateTo(PageDictionary.IncorrectCredentialsPage());
            }

            return tickets;
        }

        public async Task<TicketDto> GetTicket(Guid id)
        {
            return await _applicationClient.GetTicketAsync(id);
        }

        public async Task<TicketDto> AddCommentToTicket(string id, string text)
        {
            return await _applicationClient.AddCommentToTicketAsync(Guid.Parse(id), text);
        }

        public async Task<TicketDto> UpdateTicket(TicketDto ticketDto)
        {
            var userContext = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;
            var user = await _applicationClient.GetUserByEmailAsync(userContext.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value);
            ticketDto.ModifiedById = user.Id;
            return await _applicationClient.UpdateTicketAsync(ticketDto);
        }

        public async Task<Ticket> CreateTicket(CreateTicketDto ticket)
        {
            return await _applicationClient.CreateTicketAsync(ticket);
        }

        public async Task<string> CreateJiraIssue(Guid id)
        {
            string jiraKey = null;
            try
            {
                jiraKey = await _applicationClient.SetJiraTicketAsync(id);
            }
            catch (Exception e)
            {
                return jiraKey;
            }
            return jiraKey;
        }

        public async Task<bool> DeleteTicket(string id)
        {
            return await _applicationClient.DeleteTicketAsync(Guid.Parse(id));
        }
    }
}