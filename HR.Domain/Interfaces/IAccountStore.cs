using HR.Domain.StoreModels.AccountStoreModels;

namespace HR.Domain.Interfaces;
public interface IAccountStore
{
    AccountInfomationStoreModel? Account { get; set; }
    AccountRegisterStoreModel AccountRegisterStoreModel { get; set; }

    event Action AccountChanged;
}
