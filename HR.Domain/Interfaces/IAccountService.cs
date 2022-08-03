using HR.Domain.Models;

namespace HR.Domain.Interfaces;
public interface IAccountService
{
    void Register(AccountRegisterModel registerModel);

    TokenModel Login(AccountLoginModel loginModel);
}
