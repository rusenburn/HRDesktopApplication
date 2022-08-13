using HR.Domain.Models.DepartmentModels;

namespace HR.Domain.Interfaces;
public interface IDepartmentService
{
    Task<IEnumerable<DepartmentModel>> GetAllAsync(DepartmentQueryModel query,CancellationToken cancellationToken);
    Task<DepartmentDetailModel?> GetOneAsync(int departmentId,CancellationToken cancellationToken);
    Task<DepartmentModel?> CreateOneAsync(DepartmentCreateModel createModel,CancellationToken cancellationToken);
    Task<DepartmentModel?> UpdateOneAsync(DepartmentUpdateModel updateModel, CancellationToken cancellationToken);
    Task<bool> DeleteOneAsync(int departmentId, CancellationToken cancellationToken);
}
