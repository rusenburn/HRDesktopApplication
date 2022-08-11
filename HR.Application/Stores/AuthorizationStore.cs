using HR.Domain.Interfaces;
using HR.Domain.Models;
using System.IdentityModel.Tokens.Jwt;

namespace HR.Application.Stores;
public class AuthorizationStore : IAuthorizationStore
{
    private string? _token;
    private string? _tokenType;
    private JwtSecurityToken? _jwtSecurityToken;
    public string? Token => _token;
    public JwtSecurityToken? JwtSecurityToken => _jwtSecurityToken;
    public string? TokenType => _tokenType;

    public bool IsTokenExpired => _jwtSecurityToken == null ||
        JwtSecurityToken?.ValidFrom > DateTime.UtcNow ||
        JwtSecurityToken?.ValidTo < DateTime.UtcNow;

    public event Action<AccountInformationModel> AuthorizationChanged = (i) => { };
    public void SetAuthorizationToken(TokenModel tokenModel)
    {
        _token = tokenModel.AccessToken;
        _tokenType = tokenModel.TokenType;
        _jwtSecurityToken = new JwtSecurityToken(tokenModel.AccessToken);
        var dict = _jwtSecurityToken.Claims.ToDictionary(x => x.Type, x => x.Value);
        dict.TryGetValue("email", out string? email);
        dict.TryGetValue("sub", out string? username);
        OnAuthorizationChanged(new AccountInformationModel(email ?? "", username ?? ""));
    }
    public void Logout()
    {
        _token = null;
        _tokenType = null;
        _jwtSecurityToken = null;
        OnAuthorizationChanged(null);
    }
    protected void OnAuthorizationChanged(AccountInformationModel accountInformationModel)
    {
        AuthorizationChanged?.Invoke(accountInformationModel);
    }
}
