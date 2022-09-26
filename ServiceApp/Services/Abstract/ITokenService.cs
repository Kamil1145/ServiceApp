using ServiceApp.Models.DTO;

public interface ITokenService
{
    Task<string> GetNewAccessToken();
    Task<TokensResponseDto> GetTokens();
    Task RemoveTokens();

}


