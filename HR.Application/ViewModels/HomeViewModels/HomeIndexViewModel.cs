using HR.Application.Commands;
using HR.Application.ViewModels.AccountViewModels;
using HR.Domain.Abstracts;
using System.Windows.Input;

namespace HR.Application.ViewModels.HomeViewModels;
public class HomeIndexViewModel : ViewModelBase
{
    public ICommand AccountRegisterNavigationCommand { get; }
    public HomeIndexViewModel(INavigationCommand<AccountRegisterViewModel> accountRegisterNavigationCommand)
    {
        AccountRegisterNavigationCommand = accountRegisterNavigationCommand;
    }
}
