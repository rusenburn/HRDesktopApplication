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
    public async Task<TokenModel?> LoginAsync(AccountLoginModel loginModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(loginModel);
        try
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("username",loginModel.Username),
                new KeyValuePair<string,string>("password",loginModel.Password),
            });

            var response = await _client.PostAsync(new Uri($"{BASE_URI}/login"), content,cancellationToken);
            if (response is null || !response.IsSuccessStatusCode) return null;
            TokenModel? JWTToken = await response.Content.ReadFromJsonAsync<TokenModel>(cancellationToken:cancellationToken);
            return JWTToken;
        }
        finally
        {

        }
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
