using HR.Application.Commands;
using HR.Application.Commands.HttpCommands;
using HR.Application.ViewModels.HomeViewModels;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models;
using HR.Domain.StoreModels.AccountStoreModels;
using System.Windows.Input;

namespace HR.Application.ViewModels.AccountViewModels;
public class AccountLoginViewModel : ViewModelBase
{
    private bool _isDisposed;
    private readonly IAccountStore _accountStore;
    private readonly INavigationService<HomeIndexViewModel> _homeNavigationService;
    private readonly IFactory<IAccountService> _accountServiceFactory;
    private readonly IAuthorizationStore _authorizationStore;

    public AccountLoginStoreModel AccountLogin => _accountStore.AccountLogin;
    public AsyncCommandBase AccountLoginCommand { get; }
    public bool AccountLoginCommandCanExecute => !AccountLoginCommand.IsExecuting;
    
    public ICommand HomeIndexNavigationCommand { get; }

    public AccountLoginViewModel(IAccountStore accountStore,
                                 INavigationCommand<HomeIndexViewModel> homeIndexNavigationCommand,
                                 INavigationService<HomeIndexViewModel> homeNavigationService,
                                 IFactory<IAccountService> accountServiceFactory,
                                 IAuthorizationStore authorizationStore)
    {
        _accountStore = accountStore;
        HomeIndexNavigationCommand = homeIndexNavigationCommand;
        _homeNavigationService = homeNavigationService;
        _accountServiceFactory = accountServiceFactory;
        _authorizationStore = authorizationStore;
        AccountLoginCommand = new AsyncRelayCommand(LoginAsync);
        AccountLoginCommand.CanExecuteChanged += OnAccountLoginCommandCanExecuteChanged;
    }
    

    private void OnAccountLoginCommandCanExecuteChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(AccountLoginCommandCanExecute));
    }
    protected override void Dispose(bool disposing)
    {
        if(!_isDisposed)
        {
            if(disposing)
            {
                AccountLoginCommand.CanExecuteChanged -= OnAccountLoginCommandCanExecuteChanged;
            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
    private async Task LoginAsync(object? parameter)
    {
        using CancellationTokenSource cancelTokenSource = new();
        var cancellationToken = cancelTokenSource.Token;
        var accountService = _accountServiceFactory.Create();
        AccountLoginModel model = new(_accountStore.AccountLogin.Username, _accountStore.AccountLogin.Password);
        TokenModel? token;
        try
        {
            token = await accountService.LoginAsync(model, cancellationToken);
            if (token is null) return;
            _accountStore.JWTToken = token;
            _authorizationStore.SetAuthorizationToken(token);
            var a = _authorizationStore.IsTokenExpired;
            _homeNavigationService.Navigate();
        }
        catch (HttpRequestException e)
        {
            return;
        }
    }
}
