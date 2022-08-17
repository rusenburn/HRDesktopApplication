using HR.Domain.Models.EmployeeModels;

namespace HR.Domain.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> GetAllAsync(EmployeeQueryModel query, CancellationToken cancellationToken);
    Task<EmployeeDetailModel?> GetOneAsync(int employeeId, CancellationToken cancellationToken);
    Task<EmployeeModel?> CreateOneAsync(EmployeeCreateModel employeeCreate, CancellationToken cancellationToken);
    Task<EmployeeModel?> UpdateOneAsync(EmployeeUpdateModel employeeUpdate, CancellationToken cancellationToken);
    Task<bool> DeleteOneAsync(int employeeId, CancellationToken cancellationToken);
}
