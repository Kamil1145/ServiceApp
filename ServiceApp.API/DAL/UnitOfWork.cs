using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Models;
using ServiceApp.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private IUserRepository _userRepository;
    private IRoleRepository _rolesRepository;
    private ITicketRepository _ticketRepository;
    private ICommentRepository _commentRepository;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public IUserRepository Users
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context);
            }
            return _userRepository;
        }
    }

    public IRoleRepository Roles
    {
        get
        {
            if (_rolesRepository == null)
                _rolesRepository = new RoleRepository(_context);
            return _rolesRepository;
        }
    }

    public ITicketRepository Tickets
    {
        get
        {
            if (_ticketRepository == null)
                _ticketRepository = new TicketRepository(_context);
            return _ticketRepository;
        }
    }

    public ICommentRepository Comments
    {
        get
        {
            if (_commentRepository == null)
                _commentRepository = new CommentRepository(_context);
            return _commentRepository;
        }
    }
    public int Save()
    {
        return _context.SaveChanges();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}