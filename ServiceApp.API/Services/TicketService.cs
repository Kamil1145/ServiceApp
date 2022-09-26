using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using HtmlAgilityPack;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Services.Abstract;

class TicketService : ITicketService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TicketService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<Ticket> GetTicketById(Guid id, params Expression<Func<Ticket, object>>[] includes)
    {
        var ticket = _unitOfWork.Tickets.GetTicket(x => x.Id == id, includes);
        return ticket;
    }

    public async Task<Ticket> GetFullTicketInformation(Guid id)
    {
        var ticket = await GetTicketById(id,
            includes: new Expression<Func<Ticket, object>>[]
            {
                t => t.Comments, t => t.ResponsibleUser, t => t.CreatedBy
            });

        var comments =
            _unitOfWork.Comments.GetAll(
                filter: a => a.Ticket.Id == ticket.Id,
                includes: new Expression<Func<Comment, object>>[] { a => a.Author });
        foreach (var comment in ticket.Comments)
        {
            comment.Author = comments.Where(a => a.Id == comment.Id).SingleOrDefault().Author;
        }

        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetAllTickets(Expression<Func<Ticket, bool>>? filter = null)
    {
        return _unitOfWork.Tickets.GetAllTickets(filter: filter);
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsByUserContext(User user)
    {
        if (user.Roles.Any(x => x.RoleName == "Customer"))
        {
            return _unitOfWork.Tickets.GetAllTickets(filter: x => x.CreatedBy.Id == user.Id,
                                                     includes: new Expression<Func<Ticket, object>>[] { a => a.CreatedBy });
        }

        return _unitOfWork.Tickets.GetAllTickets(includes: new Expression<Func<Ticket, object>>[] { a => a.CreatedBy,a=>a.ResponsibleUser });
    }

    public Task DeleteTicket(Ticket ticket)
    {
        _unitOfWork.Tickets.Delete(ticket.Id);
        _unitOfWork.Save();
        return Task.CompletedTask;
    }

    public async Task<TicketDto> CreateNewTicket(CreateTicketDto ticketDto, string userMail)
    {
        var user = await _userService.GetUserByEmail(userMail);
        var ticket = _mapper.Map<CreateTicketDto, Ticket>(ticketDto);

        ticket.Id = Guid.NewGuid();
        ticket.CreatedAt = DateTime.Now;
        ticket.CreatedBy = user;
        ticket.TicketStatus = TicketStatus.Pending;

        _unitOfWork.Tickets.Insert(ticket);
        _unitOfWork.Save();

        return _mapper.Map<Ticket, TicketDto>(ticket);
    }

    public async Task<TicketDto> UpdateTicket(TicketDto ticketDto)
    {
        var modifyingUser = await _userService.GetUserById(ticketDto.ModifiedById);
        var ticket = await GetTicketById(ticketDto.Id);
        ticket.TicketStatus = ticketDto.TicketStatus;
        ticket.Description = ticketDto.Description;
        ticket.ModifiedBy = modifyingUser;
        ticket.ModifiedAt = DateTime.Now;
        ticket.DueTime = ticketDto.DueTime;
        ticket.Priority = ticketDto.Priority;
        ticket.JiraTicketId = ticketDto.JiraTicketId;
        if (ticketDto.ResponsibleUser is not null)
        {
            var user = await _userService.GetUserById(ticketDto.ResponsibleUser.Id);
            ticket.ResponsibleUser = user;
        }

        _unitOfWork.Save();
        return _mapper.Map<Ticket, TicketDto>(ticket);
    }

    public async Task<TicketDto> AssignTicketToUser(Ticket ticket, User user)
    {
        ticket.ResponsibleUser = user;
        _unitOfWork.Save();

        return _mapper.Map<Ticket, TicketDto>(ticket);
    }

    public async Task<TicketDto> AddComment(Ticket ticket, User user, string description)
    {
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Author = user,
            CreatedDate = DateTime.Now,
            Description = description,
            Ticket = ticket
        };
        _unitOfWork.Comments.Insert(comment);
        _unitOfWork.Save();

        return _mapper.Map<Ticket, TicketDto>(ticket);
    }

}