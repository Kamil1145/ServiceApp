using ServiceApp.Models.Entities;
using System.Linq.Expressions;

namespace ServiceApp.API.DAL.Interfaces;

public interface ITicketRepository
{
    IEnumerable<Ticket> GetAllTickets(
        Expression<Func<Ticket, bool>>? filter = null,
        Func<IQueryable<Ticket>, IOrderedQueryable<Ticket>>? orderBy = null,
        Expression<Func<Ticket, object>>[]? includes = null);

    Ticket GetTicket(
        Expression<Func<Ticket, bool>>? filter,
        Expression<Func<Ticket, object>>[]? includes = null);

    void Insert(Ticket entity);
    void Update(Ticket entity);
    void Delete(object entity);
}