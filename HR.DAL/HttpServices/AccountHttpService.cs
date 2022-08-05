using HR.Domain.Interfaces;
using HR.Domain.Models;
using System.Net.Http.Json;

namespace HR.DAL.HttpServices;
public class AccountHttpService : IAccountService
{
    private readonly HttpClient _client;
    private const string BASE_URI = "http://localhost:8000/users";

    public AccountHttpService(HttpClient client)
    {
        _client = client;
    }
    public Task<TokenModel?> LoginAsync(AccountLoginModel loginModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AccountInformationModel?> RegisterAsync(AccountRegisterModel registerModel,CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(registerModel,nameof(registerModel));
        try
        {
            var response = await _client.PostAsJsonAsync(new Uri($"{BASE_URI}/register"), registerModel, cancellationToken: cancellationToken);
            if (response is null || !response.IsSuccessStatusCode)
            {
                return null;
            }
            AccountInformationModel? accountInfo = await response.Content.ReadFromJsonAsync<AccountInformationModel>(cancellationToken: cancellationToken);
            return accountInfo;
        }
        finally
        {

        }
        
    }

    
}
