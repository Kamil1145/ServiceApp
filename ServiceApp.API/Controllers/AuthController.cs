using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiceApp.API.Services.Abstract;
using ServiceApp.API.Utils;
using ServiceApp.ApiClient;
using ServiceApp.Models;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace ServiceApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(
            IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserLoginDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(RegisterUser), Name = nameof(RegisterUser))]
        public async Task<ActionResult<UserLoginDto>> RegisterUser(UserRegisterDto userRegisterDto)
        {
            var user = await _userService.GetUserByEmail(userRegisterDto.Email);
            if (user is not null)
                return BadRequest($"User with email {userRegisterDto.Email} already exist");
            return Ok(await _userService.RegisterNewUser(userRegisterDto));
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TokensResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(Login), Name = nameof(Login))]
        public async Task<ActionResult<TokensResponseDto>> Login(UserLoginDto userLoginDto)
        {
            var user = await _userService.GetUserByEmail(userLoginDto.Email);

            if (user == null)
                return BadRequest("Bad username");

            if (!user.IsActive)
                return BadRequest("User Is Not Active");
            
            if (!(_tokenService.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt)))
                return BadRequest("Wrong password");

            var refreshToken = _tokenService.GenerateRefreshToken();
            await _tokenService.UpdateUserRefreshToken(refreshToken, user);
            var token = await _userService.LoginUser(user);

            
            return Ok(new TokensResponseDto()
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token
            });
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(ActivateAccount), Name = nameof(ActivateAccount))]
        public async Task<ActionResult<string>> ActivateAccount(Guid idToken)
        {
            var user = await _userService.GetUserById(idToken);
            if (user == null)
                return BadRequest("Invalid token");

            await _userService.ActivateUser(idToken, user);
            _userService.ForgottenPassword(user.Email, user);

            return Ok("user activated");
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(ForgottenPassword), Name = nameof(ForgottenPassword))]
        public async Task<ActionResult<string>> ForgottenPassword(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
                return BadRequest("Bad username");

            await _userService.ForgottenPassword(email, user);

            return Ok();
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(ResetPassword), Name = nameof(ResetPassword))]
        public async Task<ActionResult<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _tokenService.GetUserByResetToken(resetPasswordDto.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
                return BadRequest();

            await _userService.ResetPassword(resetPasswordDto, user);
            
            return Ok("Created new password");
        }

        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost(nameof(RefreshAccessToken), Name = nameof(RefreshAccessToken))]
        public async Task<ActionResult<string>> RefreshAccessToken(TokensResponseDto tokens)
        {
            if (tokens is null)
                return BadRequest("Invalid client request");

            var principal = _tokenService.GetPrincipalFromExpiredToken(tokens.AccessToken);
            var username = principal.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email).Value;

            var user = await _userService.GetUserByEmail(username);

            if (user == null || user.RefreshToken != tokens.RefreshToken || user.RefreshTokenExpires <= DateTime.Now)
                return BadRequest("Unauthorized");

            var token = await _tokenService.CreateToken(user);
                
            return Ok(token);
        }
    }
}
