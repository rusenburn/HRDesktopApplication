using HR.Domain.Interfaces;
using HR.Domain.Models.CountryModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HR.DAL.HttpServices;
public class CountryHttpService : ICountryService
{
    private readonly HttpClient _client;
    private readonly IAuthorizationStore _authorizationStore;
    private const string BASE_URI = "http://localhost:8000/countries";

    public CountryHttpService(HttpClient httpClient, IAuthorizationStore authorizationStore)
    {
        _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _authorizationStore = authorizationStore ?? throw new ArgumentNullException(nameof(authorizationStore));
    }
    public async Task<CountryModel?> CreateOneAsync(CountryCreateModel createModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(createModel);
        HttpRequestMessage message = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Post,
            Content = JsonContent.Create(createModel)
        };
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(message, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
        CountryModel? model = await response.Content.ReadFromJsonAsync<CountryModel?>(cancellationToken: cancellationToken);
        return model;
    }

    public async Task<bool> DeleteOneAsync(int countryId, CancellationToken cancellationToken)
    {
        HttpRequestMessage message = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/{countryId}"),
            Method = HttpMethod.Delete,
        };
        var response = await _client.SendAsync(message, cancellationToken);
        if (response.IsSuccessStatusCode) return true;
        return false;
    }

    public async Task<IEnumerable<CountryModel>> GetAllAsync(CountryQueryModel? query, CancellationToken cancellationToken)
    {

        var d = query?.GetDict() ?? new();
        HttpRequestMessage message = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Get,
            Content = new FormUrlEncodedContent(d)
        };
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(message, cancellationToken);
        if (!response.IsSuccessStatusCode) return Array.Empty<CountryModel>();
        var result = await response.Content.ReadFromJsonAsync<CountryModel[]>(cancellationToken: cancellationToken);
        return result ?? Enumerable.Empty<CountryModel>();

    }

    public async Task<CountryDetailModel?> GetOneAsync(int countryId, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/{countryId}"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
        var result = await response.Content.ReadFromJsonAsync<CountryDetailModel>(cancellationToken: cancellationToken);
        return result;
    }

    public async Task<CountryModel?> UpdateOneAsync(CountryUpdateModel updateModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updateModel);
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Put,
            Content = JsonContent.Create(updateModel)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
        CountryModel? result = await response.Content.ReadFromJsonAsync<CountryModel>(cancellationToken: cancellationToken);
        return result;
    }
}
