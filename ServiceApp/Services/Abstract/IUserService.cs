using ServiceApp.Models;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

public interface IUserService
{
    Task<IEnumerable<UserDto>?> GetUsers();
    Task<IEnumerable<CustomerDto>?> GetCustomers();
    Task<IEnumerable<UserDto>?> GetEmployees();
    Task<string?> LoginUser(UserLoginDto userDto);
    Task<UserLoginDto> RegisterUser(UserRegisterDto userRegisterDto);
    Task<UserDto> GetUser(Guid id);
    Task<UserDto> GetUserContext();
    Task<User> UpdateUser(UserDto userDto);
    Task<Customer> CreateCustomer(CustomerDto userDto);
    Task<string> ActivateUser(string id);
    Task<string> ResetPassword(ResetPasswordDto resetPasswordDto);

}


