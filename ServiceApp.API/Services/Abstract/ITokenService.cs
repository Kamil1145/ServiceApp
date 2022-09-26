using Microsoft.IdentityModel.Tokens;
using ServiceApp.Models;
using ServiceApp.Models.Entities;
using System.Security.Claims;

namespace ServiceApp.API.Services.Abstract
{
    public interface ITokenService
    {
        Task UpdateUserRefreshToken(RefreshToken refreshToken, User user);
        Task<string> CreateToken(User user);
        Task<User> GetUserByResetToken(string token);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordsalt);
        RefreshToken GenerateRefreshToken();
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string CreateRandomToken();
        List<Claim> CreateRoleClaims(User user);
        SigningCredentials GetSigningCredentials();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }

}
