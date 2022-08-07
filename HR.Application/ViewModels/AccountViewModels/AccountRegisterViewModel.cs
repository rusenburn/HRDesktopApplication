using HR.Application.Commands;
using HR.Application.ViewModels.HomeViewModels;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models;
using HR.Domain.StoreModels.AccountStoreModels;
using System.Windows.Input;

namespace HR.Application.ViewModels.AccountViewModels;
public class AccountRegisterViewModel : ViewModelBase
{
    private readonly IFactory<IAccountService> _accountServiceFactory;
    private readonly IAccountStore _accountStore;
    private bool _disposedValue;

    public AccountRegisterStoreModel AccountRegister => _accountStore.AccountRegisterStoreModel;
    public ICommand HomeIndexNavigationCommand { get; }
    public AsyncCommandBase AccountRegisterCommand { get; }
    public ICommand AccountLoginNavigationCommand { get; }
    public bool AccountRegisterCommandCanExecute => !AccountRegisterCommand.IsExecuting;

    public AccountRegisterViewModel(INavigationCommand<HomeIndexViewModel> homeIndexNavigationCommand,
                                    INavigationCommand<AccountLoginViewModel> accountLoginNavigationCommand,
                                    IFactory<IAccountService> accountServiceFactory,
                                    IAccountStore accountStore)
    {
        _accountStore = accountStore;
        _accountServiceFactory = accountServiceFactory;
        HomeIndexNavigationCommand = homeIndexNavigationCommand;
        AccountLoginNavigationCommand = accountLoginNavigationCommand;
        AccountRegisterCommand = new AsyncRelayCommand(RegisterAsync);
        AccountRegisterCommand.CanExecuteChanged += OnAccountLoginCommandCanExecuteChanged;
    }

    private async Task RegisterAsync(object? parameter)
    {
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var accountService = _accountServiceFactory.Create();
        var storeModel = _accountStore.AccountRegisterStoreModel;
        var model = new AccountRegisterModel(Email: storeModel.Email, Username: storeModel.Username, Password: storeModel.Password);
        var result = await accountService.RegisterAsync(model, cancellationTokenSource.Token);
        if (result is null) return;
        _accountStore.Account = new AccountInformationStoreModel(result.Email, result.Username);
        AccountLoginNavigationCommand.Execute(null);
    }


    private void OnAccountLoginCommandCanExecuteChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(AccountRegisterCommandCanExecute));
    }


    protected override void Dispose(bool disposing)
    {
        if(!_disposedValue)
        {
            if(disposing)
            {
                AccountRegisterCommand.CanExecuteChanged -= OnAccountLoginCommandCanExecuteChanged;
            }
            _disposedValue = true;
        }
        base.Dispose(disposing);
    }
}
