using HR.Domain.Interfaces;
using HR.Domain.Models;
using HR.Domain.StoreModels.AccountStoreModels;

namespace HR.Application.Commands.HttpCommands;
public class AccountRegisterCommand : AsyncCommandBase
{
    private readonly IFactory<IAccountService> _accountServiceFactory;
    private readonly IAccountStore _accountStore;

    public AccountRegisterCommand(IFactory<IAccountService> accountServiceFactory,IAccountStore accountStore)
    {
        _accountServiceFactory = accountServiceFactory;
        _accountStore = accountStore;
    }
    protected override async Task ExecuteAsync(object? parameter)
    {
        var accountService = _accountServiceFactory.Create();
        var storeModel = _accountStore.AccountRegisterStoreModel;
        var model = new AccountRegisterModel(Email: storeModel.Email, Username: storeModel.Username, Password: storeModel.Password);
        var result = await accountService.RegisterAsync(model, CancellationToken.None);
        if (result is null) return;
        _accountStore.Account = new AccountInformationStoreModel(result.Email,result.Username);
    }
}
