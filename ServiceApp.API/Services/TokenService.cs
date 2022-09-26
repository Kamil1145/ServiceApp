using Microsoft.IdentityModel.Tokens;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models;
using ServiceApp.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServiceApp.API.Services;

class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(IUnitOfWork unitOfWork,
        IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _config = config;
    }

    public SigningCredentials GetSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config.GetSection("AppSettings:Token").Value));

        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    }

    public async Task<string> CreateToken(User user)
    {
        var claims = CreateRoleClaims(user);
        var creds = GetSigningCredentials();

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(20),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User> GetUserByResetToken(string token)
        => _unitOfWork.Users.GetUser(
            filter: a => a.ResetPasswordToken == token,
            includes: new Expression<Func<User, object>>[] { a => a.Roles });


    public List<Claim> CreateRoleClaims(User user)
    {
        var userClaims = new List<Claim>()
        {
            new(ClaimTypes.Email, user.Email)
        };
        userClaims.AddRange(user.Roles.Select(role =>
            new Claim(ClaimTypes.Role, role.RoleName))
        );
        return userClaims;
    }


    public Task UpdateUserRefreshToken(RefreshToken refreshToken, User user)
    {
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenCreated = refreshToken.Created;
        user.RefreshTokenExpires = refreshToken.Expires;
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordsalt)
    {
        using (var hmac = new HMACSHA512(passwordsalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    public RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken()
        {
            Token = CreateRandomToken(),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now,
        };
    }

    public string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)),
            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature,
                StringComparison.CurrentCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}