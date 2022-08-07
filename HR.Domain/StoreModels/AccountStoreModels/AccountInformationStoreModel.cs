using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.AccountStoreModels;
public class AccountInformationStoreModel : StoreModelBase
{
    private string _email;
    private string _username;
    public string Email
    {
        get => _email;
        set
        {
            OnPropertyChanged(ref _email, value);
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

    public AccountInformationStoreModel(string email, string username)
    {
        Email = email;
        Username = username;
    }

    


}
