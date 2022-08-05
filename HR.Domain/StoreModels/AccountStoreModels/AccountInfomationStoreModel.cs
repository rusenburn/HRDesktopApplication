using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.AccountStoreModels;
public class AccountInfomationStoreModel : StoreModelBase
{
    private string _email;
    private string _username;
    public string Email
    {
        get => _email;
        set
        {
            if (_email == value) return;

            _email = value;
            OnPropertyChanged(nameof(Email));
        }

    }
    public string Username
    {
        get => _username;
        set
        {
            if (_username == value) return;
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public AccountInfomationStoreModel(string email, string username)
    {
        Email = email;
        Username = username;
    }

    


}
