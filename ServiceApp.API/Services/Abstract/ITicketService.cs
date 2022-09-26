using System.Linq.Expressions;
using ServiceApp.Models;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Services.Abstract
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTickets(Expression<Func<Ticket, bool>>? filter);
        Task<IEnumerable<Ticket>> GetAllTicketsByUserContext(User user);
        Task<Ticket> GetTicketById(Guid id, params Expression<Func<Ticket, object>>[] includes);
        Task<Ticket> GetFullTicketInformation(Guid id);
        Task<TicketDto> CreateNewTicket(CreateTicketDto ticketDto, string userMail);
        Task<TicketDto> AssignTicketToUser(Ticket ticket, User user);
        Task<TicketDto> UpdateTicket(TicketDto ticket);
        Task<TicketDto> AddComment(Ticket ticket, User user, string description);
        Task DeleteTicket(Ticket ticket);
    }
}
