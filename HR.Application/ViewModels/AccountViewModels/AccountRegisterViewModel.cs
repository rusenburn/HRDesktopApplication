using HR.Application.Commands;
using HR.Application.Commands.HttpCommands;
using HR.Application.ViewModels.HomeViewModels;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.StoreModels.AccountStoreModels;
using System.Windows.Input;

namespace HR.Application.ViewModels.AccountViewModels;
public class AccountRegisterViewModel : ViewModelBase
{

    private readonly IAccountStore _accountStore;
    public AccountRegisterStoreModel AccountRegister => _accountStore.AccountRegisterStoreModel;
    public ICommand HomeIndexNavigationCommand { get; }
    public ICommand AccountRegisterCommand { get; }

    public AccountRegisterViewModel(INavigationCommand<HomeIndexViewModel> homeIndexNavigationCommand,
            IAccountStore accountStore, AccountRegisterCommand accountRegisterCommand)
    {
        _accountStore = accountStore;
        HomeIndexNavigationCommand = homeIndexNavigationCommand;
        AccountRegisterCommand = accountRegisterCommand;
    }
}
