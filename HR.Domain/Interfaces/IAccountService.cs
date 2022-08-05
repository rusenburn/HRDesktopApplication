using HR.Domain.Models;

namespace HR.Domain.Interfaces;
public interface IAccountService
{
    Task<AccountInformationModel?> RegisterAsync(AccountRegisterModel registerModel,CancellationToken cancellationToken);

    Task<TokenModel?> LoginAsync(AccountLoginModel loginModel,CancellationToken cancellationToken);
}
