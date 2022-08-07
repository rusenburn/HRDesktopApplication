using HR.Domain.Interfaces;
using HR.Domain.Models;

namespace HR.Application.Commands.HttpCommands;
public class AccountLoginCommand : AsyncCommandBase
{
    private readonly IAccountStore _accountStore;
    private readonly IFactory<IAccountService> _accountServiceFactory;

    public AccountLoginCommand(IAccountStore accountStore, IFactory<IAccountService> accountServiceFactory)
    {
        _accountStore = accountStore;
        _accountServiceFactory = accountServiceFactory;
    }

    protected override async Task ExecuteAsync(object? parameter)
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
        }
        catch (HttpRequestException e)
        {
            return;
        }
    }
}
