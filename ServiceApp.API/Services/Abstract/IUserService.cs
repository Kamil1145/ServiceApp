using ServiceApp.Models;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Services.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByEmail(string email);
        Task<UserDto> RegisterNewUser(UserRegisterDto newUserDto);
        Task<string> LoginUser(User userFromReguest);
        Task ResetPassword(ResetPasswordDto resetPasswordDto, User user);
        Task ActivateUser(Guid token, User user);
        Task ForgottenPassword(string email, User user);
        void DeleteUser(Guid userId);
        Task<User> UpdateUser(UserDto userDto);
        Task<User> ModifyUserActivityAndRoles(int? roleId, bool? isActive, User user);
        Task<User> CreateOrUpdateJiraCredentials(User? user, string login, string password);
        Task<User> SetUserPassword(Guid customerId, string password);
        Task<IEnumerable<UserDto>> GetEmployees();
    }


}
