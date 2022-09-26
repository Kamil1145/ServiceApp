using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace ServiceApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IJiraService _jiraService;

        public TicketController(
            ITicketService ticketService,
            IUserService userService,
            IJiraService jiraService)
        {
            _ticketService = ticketService;
            _userService = userService;
            _jiraService = jiraService;
        }

        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Ticket>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet(nameof(GetTickets), Name = nameof(GetTickets))]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            string email = null;
            try
            {
                email = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email).Value;
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
            var userContext = await _userService.GetUserByEmail(email);

            try
            {
                return Ok(await _ticketService.GetAllTicketsByUserContext(userContext));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TicketDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet(nameof(GetTicket), Name = nameof(GetTicket))]
        public async Task<ActionResult<TicketDto>> GetTicket(Guid id)
        {
            try
            {
                return Ok(await _ticketService.GetFullTicketInformation(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Ticket))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost(nameof(CreateTicket), Name = nameof(CreateTicket))]
        public async Task<ActionResult<Ticket>> CreateTicket(CreateTicketDto createTicktetDto)
        {
            var userMail = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email).Value;

            try
            {
                if (createTicktetDto == null)
                    return BadRequest();

                await _ticketService.CreateNewTicket(createTicktetDto, userMail);

                return Ok(createTicktetDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TicketDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(AssignTicketToUser), Name = nameof(AssignTicketToUser))]
        public async Task<ActionResult<TicketDto>> AssignTicketToUser(Guid ticketId, Guid userId)
        {
            var user = await _userService.GetUserById(userId);
            var ticket = await _ticketService.GetTicketById(ticketId, null);

            if (ticket is null || user is null)
                return BadRequest();

            return Ok(await _ticketService.AssignTicketToUser(ticket, user));
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TicketDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(string))]
        [HttpPost(nameof(UpdateTicket), Name = nameof(UpdateTicket))]
        public async Task<ActionResult<TicketDto>> UpdateTicket(TicketDto ticketDto)
        {
            var ticket = await _ticketService.GetTicketById(ticketDto.Id);

            if (ticket is null)
                return NotFound("Error");

            return Ok(await _ticketService.UpdateTicket(ticketDto));
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(bool))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(string))]
        [HttpDelete(nameof(DeleteTicket), Name = nameof(DeleteTicket))]
        public async Task<ActionResult<TicketDto>> DeleteTicket(Guid guid)
        {
            var ticket = await _ticketService.GetTicketById(guid);

            if (ticket is null)
            {
                return NotFound("Ticket not found");
            }

            await _ticketService.DeleteTicket(ticket);
            return Ok(true);
        }


        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(SetJiraTicket), Name = nameof(SetJiraTicket))]
        public async Task<ActionResult<string>> SetJiraTicket(Guid ticketId)
        {
            TicketDto ticketDto = null;
            var userMail = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email).Value;
            var user = await _userService.GetUserByEmail(userMail);

            var ticket = await _ticketService.GetTicketById(ticketId);
            if (string.IsNullOrEmpty(ticket.JiraTicketId))
            {
                _jiraService._userContext = user;
                ticketDto = await _jiraService.CreateJiraTicket(ticket);
            }

            if (ticket !=null && ticket.JiraTicketId != null)
                return Ok(ticket.JiraTicketId);


            return BadRequest("Not Created");
        }

        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TicketDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(AddCommentToTicket), Name = nameof(AddCommentToTicket))]
        public async Task<ActionResult<TicketDto>> AddCommentToTicket(Guid ticketId, string description)
        {
            var userMail = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email).Value;

            var user = await _userService.GetUserByEmail(userMail);
            var ticket = await _ticketService.GetTicketById(ticketId, null);

            if (ticket is null || user is null)
                return BadRequest();

            return Ok(await _ticketService.AddComment(ticket, user, description));
        }

    }

}
