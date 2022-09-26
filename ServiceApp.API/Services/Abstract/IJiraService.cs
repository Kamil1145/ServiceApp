using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using ServiceApp.Models.JiraEntities;

namespace ServiceApp.API.Services.Abstract
{
    public interface IJiraService
    {
        User _userContext{ get; set; }
        Task<JiraProjectResponse> GetJiraProjectIssues();
        Task<TicketDto> CreateJiraTicket(Ticket ticket);
    }

}
