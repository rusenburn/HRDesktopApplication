using HR.Domain.Models;
using HR.Domain.StoreModels.AccountStoreModels;

namespace HR.Domain.Interfaces;
public interface IAccountStore : IDisposable
{
    AccountInformationStoreModel? Account { get; set; }
    AccountRegisterStoreModel AccountRegisterStoreModel { get; }
    AccountLoginStoreModel AccountLogin { get; }
    TokenModel? JWTToken { get; set; }

    event Action AccountChanged;

}
