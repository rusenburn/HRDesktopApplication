using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.AccountStoreModels;
public class AccountLoginStoreModel : StoreModelBase
{
    private string _username="";

    public string Username
    {
        get { return _username; }
        set
        {
            //_username = value;
            //OnPropertyChanged(nameof(Username));
            OnPropertyChanged(ref _username, value);
        }
    }

    private string _password="";

    public string Password
    {
        get { return _password; }
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
}
