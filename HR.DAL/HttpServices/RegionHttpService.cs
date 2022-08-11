using HR.Domain.Interfaces;
using HR.Domain.Models.RegionModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HR.DAL.HttpServices;
public class RegionHttpService : IRegionService
{
    private readonly HttpClient _client;
    private readonly IAuthorizationStore _authorizationStore;
    private const string BASE_URI = "http://localhost:8000/regions";
    public RegionHttpService(HttpClient client, IAuthorizationStore authorizationStore)
    {
        _client = client;
        _authorizationStore = authorizationStore;
    }
    public async Task<RegionDetailModel?> CreateOneAsync(RegionCreateModel regionCreateModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(regionCreateModel);
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Post,
            Content = JsonContent.Create(regionCreateModel),
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode) return null;
        var result = await response.Content.ReadFromJsonAsync<RegionDetailModel>(cancellationToken: cancellationToken);
        return result;
    }

    public async Task<bool> DeleteOneAsync(int regionId, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new HttpRequestMessage()
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{BASE_URI}/{regionId}")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode) return true;
        return false;
    }

    public async Task<IEnumerable<RegionModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{BASE_URI}/")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);

        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return Array.Empty<RegionModel>();
        var regions = await response.Content.ReadFromJsonAsync<RegionModel[]>(cancellationToken: cancellationToken);
        return regions ?? Array.Empty<RegionModel>();
        ;
    }

    public async Task<RegionDetailModel?> GetOneAsync(int regionId, CancellationToken cancellationToken)
    {

        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/{regionId}"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);

        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;

        RegionDetailModel? regionDetailModel = await response.Content.ReadFromJsonAsync<RegionDetailModel>(cancellationToken: cancellationToken);
        return regionDetailModel;

    }

    public async Task<RegionDetailModel?> UpdateOneAsync(RegionUpdateModel regionUpdateModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(regionUpdateModel);

        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Put,
            Content = JsonContent.Create(regionUpdateModel)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        RegionDetailModel? regionDetailModel = await response.Content.ReadFromJsonAsync<RegionDetailModel>(cancellationToken: cancellationToken);
        return regionDetailModel;
    }
}
