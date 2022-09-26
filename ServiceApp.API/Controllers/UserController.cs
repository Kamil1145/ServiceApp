using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ServiceApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(
        IMapper mapper,
        IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [Authorize(Roles ="Admin,Developer,Support")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpGet(nameof(GetUsers), Name = nameof(GetUsers))]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            return Ok(await _userService.GetAllUsers());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }
    }
    
    [Authorize]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpGet(nameof(GetEmployees), Name = nameof(GetEmployees))]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetEmployees()
    {
        try
        {
            return Ok(await _userService.GetEmployees());
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpGet(nameof(GetUser), Name = nameof(GetUser))]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        try
        {
            var result = await _userService.GetUserById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<User, UserDto>(result));
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpGet(nameof(GetUserByEmail), Name=nameof(GetUserByEmail))]
    public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
    {
        try
        {
            var result = await _userService.GetUserByEmail(email);

            if (result == null)
            {
                return NotFound();
            }

            return _mapper.Map<User, UserDto>(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
        }
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpPost(nameof(CreateUser),Name = nameof(CreateUser))]
    public async Task<ActionResult<UserDto>> CreateUser(UserRegisterDto userDto)
    {
        try
        {
            var user = await _userService.GetUserByEmail(userDto.Email);
            if (user is not null)
                return BadRequest($"User with {userDto.Email} already exist");

            var newUser = await _userService.RegisterNewUser(userDto);

            return Ok(newUser);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
        }
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpPost(nameof(UpdateJiraAccount), Name = nameof(UpdateJiraAccount))]
    [Authorize]
    public async Task<ActionResult<User>> UpdateJiraAccount(string login, string password)
    {
        var userEmail = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email).Value;
        try
        {
            var user = await _userService.GetUserByEmail(userEmail);
            if (user is null)
                return BadRequest($"User with {userEmail} does not exist");

            await _userService.CreateOrUpdateJiraCredentials(user, login, password);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
        }
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpPut(nameof(UpdateUserRoleAndActivity), Name = nameof(UpdateUserRoleAndActivity))]
    public async Task<ActionResult<User>> UpdateUserRoleAndActivity(int? id, Guid userId, bool? isActive)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null)
            return BadRequest("user does not exit");

        var modifiedUser = await _userService.ModifyUserActivityAndRoles(id, isActive, user);

        return Ok(modifiedUser);
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpPost(nameof(UpdateUser), Name = nameof(UpdateUser))]
    public async Task<ActionResult<User>> UpdateUser(UserDto userDto)
    {
        // do poprawki
        var user = await _userService.GetUserById(userDto.Id);
        if (user == null)
            return BadRequest("user does not exit");

        var newUser = await _userService.UpdateUser(userDto);

        return Ok(newUser);
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPut(nameof(SetUserPassword), Name = nameof(SetUserPassword))]
    public async Task<ActionResult<User>> SetUserPassword(Guid userId, string password)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null)
            return BadRequest("user does not exit");

        var modifiedUser = await _userService.SetUserPassword(userId, password);

        return Ok(modifiedUser);
    }

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpDelete(nameof(DeleteUser), Name = nameof(DeleteUser))]
    public async Task<ActionResult<User>> DeleteUser(Guid id)
    {
        try
        {
            var UserToDelete = await _userService.GetUserById(id);
            if (UserToDelete == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            _userService.DeleteUser(UserToDelete.Id);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
        }
    }
}

