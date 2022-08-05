using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.AccountStoreModels;
public class AccountRegisterStoreModel : StoreModelBase
{
    private string _email="";
    private string _username="";
    private string _password="";
    public string Email
    {
        get => _email;
        set
        {
            if (_email == value) return;
            _email = value;
            OnPropertyChanged(Email);
        }
    }
    public string Username
    {
        get { return _username; }
        set
        {
            if (_username == value) return;
            _username = value;
            OnPropertyChanged(Username);
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (_password == value) return;
            _password = value;
            OnPropertyChanged(Password);
        }
    }

    public AccountRegisterStoreModel(string email, string username, string password)
    {
        Email = email;
        Username = username;
        Password = password;
    }
}
