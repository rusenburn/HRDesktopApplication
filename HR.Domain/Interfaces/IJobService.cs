using HR.Domain.Models.JobModels;

namespace HR.Domain.Interfaces;
public interface IJobService
{
    Task<IEnumerable<JobModel>> GetAllAsync(JobQueryModel query,CancellationToken cancellationToken);
    Task<JobDetailModel?> GetOneAsync(int jobId,CancellationToken cancellationToken);
    Task<JobModel?> CreateOneAsync(JobCreateModel jobCreate,CancellationToken cancellationToken);
    Task<JobModel?> UpdateOneAsync(JobUpdateModel jobUpdate,CancellationToken cancellationToken);
    Task<bool> DeleteOneAsync(int jobId, CancellationToken cancellationToken);
}
