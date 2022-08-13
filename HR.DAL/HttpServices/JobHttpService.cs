using HR.Domain.Interfaces;
using HR.Domain.Models.JobModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HR.DAL.HttpServices;
public class JobHttpService : IJobService
{
    private readonly HttpClient _client;
    private readonly IAuthorizationStore _authorizationStore;
    private const string BASE_URI = "http://localhost:8000/jobs";
    public JobHttpService(HttpClient client, IAuthorizationStore authorizationStore)
    {
        _client = client;
        _authorizationStore = authorizationStore;
    }

    public async Task<JobModel?> CreateOneAsync(JobCreateModel jobCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(jobCreate);
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Post,
            Content = JsonContent.Create(jobCreate)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        var result = await response.Content.ReadFromJsonAsync<JobModel>(cancellationToken: cancellationToken);
        return result;
    }

    public async Task<bool> DeleteOneAsync(int jobId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/{jobId}"),
            Method = HttpMethod.Delete,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<JobModel>> GetAllAsync(JobQueryModel? query, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Delete,
        };
        if (query is not null)
            request.Content = new FormUrlEncodedContent(query.GetDict());
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return Enumerable.Empty<JobModel>();
        var result = await response.Content.ReadFromJsonAsync<JobModel[]>(cancellationToken: cancellationToken);
        return result ?? Enumerable.Empty<JobModel>();
    }

    public async Task<JobDetailModel?> GetOneAsync(int jobId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/{jobId}"),
            Method = HttpMethod.Get,
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        var result = await response.Content.ReadFromJsonAsync<JobDetailModel>(cancellationToken: cancellationToken);
        return result;
    }

    public async Task<JobModel?> UpdateOneAsync(JobUpdateModel jobUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(jobUpdate);
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Put,
            Content = JsonContent.Create(jobUpdate)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        var result = await response.Content.ReadFromJsonAsync<JobModel?>();
        return result;
    }
}
