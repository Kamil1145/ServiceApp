using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetTickets();
    Task<TicketDto> GetTicket(Guid id);
    Task<Ticket> CreateTicket(CreateTicketDto ticket);
    Task<TicketDto> AddCommentToTicket(string id, string text);
    Task<TicketDto> UpdateTicket(TicketDto ticketDto);
    Task<string> CreateJiraIssue(Guid id);
    Task<bool> DeleteTicket(string id);
}
