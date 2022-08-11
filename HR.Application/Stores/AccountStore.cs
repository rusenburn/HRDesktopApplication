using HR.Domain.Interfaces;
using HR.Domain.Models;
using HR.Domain.StoreModels.AccountStoreModels;

namespace HR.Application.Stores;
public class AccountStore : IAccountStore
{
    private bool _isDisposedValue;
    private AccountInformationStoreModel? _account = null;
    private readonly AccountRegisterStoreModel _accountRegister = new("", "", "");
    private readonly AccountLoginStoreModel _accountLogin = new();
    private readonly IAuthorizationStore _authorizationStore;
    private TokenModel? _jwtToken = null;
    public TokenModel? JWTToken
    {
        get => _jwtToken; set
        {
            _jwtToken = value;
        }
    }
    public AccountLoginStoreModel AccountLogin
    {
        get => _accountLogin;
    }

    public AccountRegisterStoreModel AccountRegisterStoreModel
    {
        get => _accountRegister;
    }
    public AccountInformationStoreModel? Account
    {
        get => _account;
        set
        {
            if (_account == value) return;
            _account = value;
            OnAccountChanged();
        }
    }
    public event Action AccountChanged = () => { };

    public AccountStore(IAuthorizationStore authorizationStore)
    {
        _authorizationStore = authorizationStore;
        _authorizationStore.AuthorizationChanged += OnAuthorizationChanged;
    }
    protected void OnAccountChanged()
    {
        AccountChanged?.Invoke();
    }
    protected void OnAuthorizationChanged(AccountInformationModel? accountInfo)
    {
        _accountLogin.Password = "";
        if(accountInfo is not null)
        {
            _accountLogin.Username = accountInfo.Username;
        }
        
    }

    public void Dispose()
    {
        
        if (!_isDisposedValue)
        {
            _authorizationStore.AuthorizationChanged -= OnAuthorizationChanged;
            _isDisposedValue = true;
            GC.SuppressFinalize(this);
        }
    }
}
