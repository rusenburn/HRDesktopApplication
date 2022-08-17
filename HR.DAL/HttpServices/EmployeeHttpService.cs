using HR.Domain.Interfaces;
using HR.Domain.Models.EmployeeModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HR.DAL.HttpServices;
public class EmployeeHttpService : IEmployeeService
{
    private readonly IAuthorizationStore _authorizationStore;
    private readonly HttpClient _client;
    private const string BASE_URI = "http://localhost:8000/employees";
    public EmployeeHttpService(IAuthorizationStore authorizationStore, HttpClient client)
    {
        _authorizationStore = authorizationStore;
        _client = client;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationStore.Token);
    }

    public async Task<EmployeeModel?> CreateOneAsync(EmployeeCreateModel employeeCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(employeeCreate);
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Post,
            Content = JsonContent.Create(employeeCreate),
        };
        var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode == false) return null;
        var employee = await response.Content.ReadFromJsonAsync<EmployeeModel>(cancellationToken: cancellationToken);
        return employee;
    }

    public async Task<bool> DeleteOneAsync(int employeeId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/{employeeId}"),
            Method = HttpMethod.Delete,
        };
        var response = await _client.SendAsync(request,cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<EmployeeModel>> GetAllAsync(EmployeeQueryModel query, CancellationToken cancellationToken)
    {
        HttpRequestMessage message = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{BASE_URI}/"),
            Content = new FormUrlEncodedContent(query.GetDict())
        };
        var response = await _client.SendAsync(message, cancellationToken);
        if (response.IsSuccessStatusCode == false) return Enumerable.Empty<EmployeeModel>();
        var result = await response.Content.ReadFromJsonAsync<EmployeeModel[]>(cancellationToken: cancellationToken);
        return result ?? Enumerable.Empty<EmployeeModel>();
    }

    public async Task<EmployeeDetailModel?> GetOneAsync(int employeeId, CancellationToken cancellationToken)
    {
        HttpRequestMessage message = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{BASE_URI}/{employeeId}"),
        };
        var response = await _client.SendAsync(message, cancellationToken);
        if (response.IsSuccessStatusCode == false) return null;
        var result = await response.Content.ReadFromJsonAsync<EmployeeDetailModel>(cancellationToken: cancellationToken);
        return result;
    }

    public async Task<EmployeeModel?> UpdateOneAsync(EmployeeUpdateModel employeeUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(employeeUpdate);
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{BASE_URI}/"),
            Method = HttpMethod.Put,
            Content = JsonContent.Create(employeeUpdate),
        };
        var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode == false) return null;
        var employee = await response.Content.ReadFromJsonAsync<EmployeeModel>(cancellationToken: cancellationToken);
        return employee;
    }
}
