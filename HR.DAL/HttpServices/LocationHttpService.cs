using HR.Domain.Interfaces;
using HR.Domain.Models.LocationModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HR.DAL.HttpServices;
public class LocationHttpService : ILocationService
{
    private readonly HttpClient _client;
    private readonly IAuthorizationStore _authorizationStore;
    private const string BASE_URI = "http://localhost:8000/locations";

    public LocationHttpService(HttpClient client, IAuthorizationStore authorizationStore)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _authorizationStore = authorizationStore ?? throw new ArgumentNullException(nameof(authorizationStore));
    }

    public async Task<IEnumerable<LocationModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);

        var response = await _client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return Enumerable.Empty<LocationModel>();

        var result = await response.Content.ReadFromJsonAsync<LocationModel[]>(cancellationToken: cancellationToken);
        return result ?? Enumerable.Empty<LocationModel>();
    }

    public async Task<LocationDetailModel?> GetOneAsync(int LocationId, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new(HttpMethod.Get, new Uri($"{BASE_URI}/{LocationId}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);

        var response = await _client.SendAsync(request,cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
        var result = await response.Content.ReadFromJsonAsync<LocationDetailModel>(cancellationToken:cancellationToken);
        return result;
    }

    public async Task<LocationModel?> CreateOneAsync(LocationCreateModel createModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(createModel);
        HttpRequestMessage request = new()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Post,
            Content = JsonContent.Create(createModel)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var respose = await _client.SendAsync(request,cancellationToken);
        if (respose.IsSuccessStatusCode == false) return null;
        var result = await respose.Content.ReadFromJsonAsync<LocationModel>();
        return result;

    }

    public async Task<LocationModel?> UpdateOneAsync(LocationUpdateModel updateModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updateModel);
        HttpRequestMessage request = new()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Put,
            Content = JsonContent.Create(updateModel),
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request,cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        LocationModel? result = await response.Content.ReadFromJsonAsync<LocationModel>();
        return result;

    }

    public async Task<bool> DeleteOneAsync(int locationId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Delete,
            RequestUri =new Uri($"{BASE_URI}/{locationId}")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request,cancellationToken);
        return response.IsSuccessStatusCode;

    }
}
