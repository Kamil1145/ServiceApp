using Microsoft.AspNetCore.Mvc;
using ServiceApp.API.Services;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace ServiceApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        public CustomerController(
            ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet(nameof(GetCustomer), Name = nameof(GetCustomer))]
        public async Task<ActionResult<UserDto>> GetCustomer(Guid id)
        {
            try
            {
                var result = await _customerService.GetCustomer(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet(nameof(GetAllCustomers), Name = nameof(GetAllCustomers))]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            try
            {
                var result = await _customerService.GetAllCustomers();

                if (result == null)
                {
                    return NotFound("Error retrieving data from the database");
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Customer))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(CreateCustomer), Name = nameof(CreateCustomer))]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            try
            {
                if (customerDto == null)
                    return BadRequest();
                
                var user = await _userService.GetUserByEmail(customerDto.Email);
                if (user is not null)
                    return BadRequest($"User with email {customerDto.Email} already exist");

                var customer = await _customerService.CreateNewCustomer(customerDto);

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

    }


}