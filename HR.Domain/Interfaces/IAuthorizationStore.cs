using HR.Domain.Models;
using System.IdentityModel.Tokens.Jwt;

namespace HR.Domain.Interfaces;
public interface IAuthorizationStore
{
    string? Token { get; }
    string? TokenType { get; }
    JwtSecurityToken? JwtSecurityToken { get; }
    bool IsTokenExpired { get; }

    void SetAuthorizationToken(TokenModel tokenModel);
    void Logout();

    event Action<AccountInformationModel> AuthorizationChanged;

}
