using ServiceApp.API.Models;
using ServiceApp.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    ITicketRepository Tickets { get; }
    ICommentRepository Comments { get; }
    int Save();
}