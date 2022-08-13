using HR.Domain.Interfaces;
using HR.Domain.Models.DepartmentModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;

namespace HR.DAL.HttpServices;
public class DepartmentHttpService : IDepartmentService
{
    private readonly HttpClient _client;
    private readonly IAuthorizationStore _authorizationStore;
    private const string BASE_URI = "http://localhost:8000/departments";

    public DepartmentHttpService(HttpClient client, IAuthorizationStore authorizationStore)
    {
        _client = client;
        _authorizationStore = authorizationStore;
    }

    public async Task<DepartmentModel?> CreateOneAsync(DepartmentCreateModel createModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(createModel);
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{BASE_URI}/"),
            Content = JsonContent.Create(createModel)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        var result = await response.Content.ReadFromJsonAsync<DepartmentModel>(cancellationToken: cancellationToken);
        return result;
    }

    public async Task<bool> DeleteOneAsync(int departmentId, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{BASE_URI}/{departmentId}")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);

        var response = await _client.SendAsync(request, cancellationToken);

        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<DepartmentModel>> GetAllAsync(DepartmentQueryModel? query, CancellationToken cancellationToken)
    {

        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        if (query is not null)
            request.Content = new FormUrlEncodedContent(query.GetDict());
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false)
            return Enumerable.Empty<DepartmentModel>();
        var result = await response.Content.ReadFromJsonAsync<DepartmentModel[]>(cancellationToken: cancellationToken);
        return result ?? Enumerable.Empty<DepartmentModel>();

    }

    public async Task<DepartmentDetailModel?> GetOneAsync(int departmentId, CancellationToken cancellationToken)
    {
        HttpRequestMessage request = new HttpRequestMessage
        {
            RequestUri = new Uri($"{BASE_URI}/{departmentId}"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);

        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;

        var result = await response.Content.ReadFromJsonAsync<DepartmentDetailModel>(cancellationToken: cancellationToken);
        return result;

    }

    public async Task<DepartmentModel?> UpdateOneAsync(DepartmentUpdateModel updateModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updateModel);
        HttpRequestMessage request = new()
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"{BASE_URI}/"),
            Content = JsonContent.Create(updateModel)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        var result = await response.Content.ReadFromJsonAsync<DepartmentModel?>(cancellationToken:cancellationToken);
        return result;
    }
}
