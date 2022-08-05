using HR.Domain.Interfaces;
using HR.Domain.StoreModels.AccountStoreModels;

namespace HR.Application.Stores;
public class AccountStore : IAccountStore
{
    private AccountInfomationStoreModel? _account = null;
    private AccountRegisterStoreModel _accountRegister = new("", "", "");
    public AccountRegisterStoreModel AccountRegisterStoreModel
    {
        get => _accountRegister;
        set { _accountRegister = value; }
    }
    public AccountInfomationStoreModel? Account
    {
        get => _account; set
        {
            if (_account == value) return;
            _account = value;
            OnAccountChanged();
        }
    }
    public event Action AccountChanged = () => { };
    protected void OnAccountChanged()
    {
        AccountChanged?.Invoke();
    }
}
