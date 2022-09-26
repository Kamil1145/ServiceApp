using Microsoft.IdentityModel.Tokens;
using ServiceApp.API.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using ServiceApp.Models;
using ServiceApp.API.DAL.Interfaces;

namespace ServiceApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly ITokenService _tokenService;

        public UserService(
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRoleService roleService,
            ITokenService tokenService)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleService = roleService;
            _tokenService = tokenService;
        }

        public async Task<User> GetUserById(Guid id)
            => _unitOfWork.Users.GetUser(
                filter: a => a.Id == id,
                includes: new Expression<Func<User, object>>[] { a => a.Roles, a => a.TicketsCreated, a => a.JiraCredentials });

        public async Task<User> GetUserByEmail(string email)
            => _unitOfWork.Users.GetUser(
                filter: a => a.Email == email,
                includes: new Expression<Func<User, object>>[] { a => a.Roles, a => a.TicketsCreated, a => a.JiraCredentials });

        public async Task<IEnumerable<UserDto>> GetAllUsers()
            => _unitOfWork.Users.GetAllUsers(
                    includes: new Expression<Func<User, object>>[] { a => a.Roles })
                .Select(a => _mapper.Map<User, UserDto>(a));

        public async Task<string> LoginUser(User userFromReguest)
        {
            string token = await _tokenService.CreateToken(userFromReguest);
            return token;
        }

        public async Task<User> ModifyUserActivityAndRoles(int? roleId, bool? isActive, User user)
        {
            if (isActive is not null)
                user.IsActive = (bool)isActive;

            if (roleId is not null)
            {
                var role = _roleService.GetRoles()
                    .Where(r => r.Id == roleId)
                    .FirstOrDefault();

                user.Roles.Add(role);
            }

            _unitOfWork.Save();
            return user;
        }

        public async Task<User> CreateOrUpdateJiraCredentials(User? user, string login, string password)
        {
            JiraCredentials jiraCredentials = new JiraCredentials()
            {
                Id = Guid.NewGuid(),
                User = user,
                Username = login,
                Password = password
            };

            user.JiraCredentials = jiraCredentials;

            _unitOfWork.Save();
            return user;
        }

        public async Task<User> UpdateUser(UserDto userDto)
        {
            var user = await GetUserById(userDto.Id);

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.IsActive = userDto.IsActive;

            var rolesList = _roleService.GetRoles();
            List<Role> userRoles = new List<Role>();

            foreach (var userDtoRole in userDto.Roles)
            {
                userRoles.Add(rolesList.FirstOrDefault(x => x.Id == userDtoRole.Id));
            }

            user.Roles = userRoles;

            _unitOfWork.Save();
            return user;
        }


        public async Task<User> SetUserPassword(Guid customerId, string password)
        {
            _tokenService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = await GetUserById(customerId);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _unitOfWork.Save();
            return user;
        }

        public void DeleteUser(Guid userId)
        {
            _unitOfWork.Users.Delete(userId);
            _unitOfWork.Save();
        }

        public async Task<UserDto> RegisterNewUser(UserRegisterDto newUserDto)
        {
            _tokenService.CreatePasswordHash(newUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = _mapper.Map<UserRegisterDto, User>(newUserDto);
            newUser.Id = Guid.NewGuid();
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;
            newUser.CreatedAt = DateTime.UtcNow;

            if (newUserDto.RoleId != default)
            {
                var role = _roleService
                    .GetRoles()
                    .FirstOrDefault(r => r.Id == newUserDto.RoleId);

                newUser.Roles.Add(role);
            }

            _unitOfWork.Users.Insert(newUser);
            _unitOfWork.Save();

            _emailService.SendEmail("topic",
                $"hello {newUser.FirstName} {newUser.LastName}",
                newUser.Email,
                $"Siemanko nowy użytkowniku" +
                $"{newUser.FirstName} {newUser.LastName}" +
                $"twoje id: {newUser.Id}");

            return _mapper.Map<User, UserDto>(newUser);
        }


        public Task ResetPassword(ResetPasswordDto resetPasswordDto, User user)
        {
            _tokenService.CreatePasswordHash(resetPasswordDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            user.ResetPasswordToken = null;
            user.ResetTokenExpires = null;

            _unitOfWork.Save();

            return Task.CompletedTask;
        }


        public Task ActivateUser(Guid token, User user)
        {
            user.IsActive = true;
            _unitOfWork.Save();

            return Task.CompletedTask;
        }

        public Task ForgottenPassword(string email, User user)
        {
            user.ResetPasswordToken = _tokenService.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _unitOfWork.Save();

            _emailService.SendEmail("Create new password",
                $" <address>https://localhost:44334/resetpassword/{user.ResetPasswordToken}</address>",
                user.Email, user.FirstName);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<UserDto>> GetEmployees()
        {
            var roles = _roleService.GetRoles().Where(x => x.RoleName != "Customer").ToList();
            List<User> employees = new List<User>();

            var users = _unitOfWork.Users.GetAllUsers(includes: new Expression<Func<User, object>>[] { a => a.Roles });

            foreach (var user in users)
            {
                if (!user.Roles.Any(x=>x.RoleName == "Customer"))
                {
                    employees.Add(user);
                }
            }

            return employees.Select(a => _mapper.Map<User, UserDto>(a));


        }
    }
}
