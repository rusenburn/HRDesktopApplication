using HR.Application.Commands;
using HR.Application.ViewModels.HomeViewModels;
using HR.Domain.Abstracts;
using System.Windows.Input;

namespace HR.Application.ViewModels.AccountViewModels;
public class AccountRegisterViewModel : ViewModelBase
{
    private string _password = "";

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public ICommand HomeIndexNavigationCommand { get; }

    public string Email { get; set; } = "";
    public string Username { get; set; } = "";
    public AccountRegisterViewModel(INavigationCommand<HomeIndexViewModel> homeIndexNavigationCommand)
    {
        HomeIndexNavigationCommand = homeIndexNavigationCommand;
    }
}
