using HR.Domain.Abstracts;

namespace HR.Application.StoreModels;
public class AccountInformationStoreModel : StoreModelBase
{
    private string _username;
    private string _email;

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
            if (value == _username) return;
            _username = value;
            OnPropertyChanged(nameof(_username));
        }
    }
    public AccountInformationStoreModel(string email, string username)
    {
        Username = username;
        Email = email;
    }





}
