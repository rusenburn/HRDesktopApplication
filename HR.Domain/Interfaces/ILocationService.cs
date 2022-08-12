using HR.Domain.Models.LocationModels;

namespace HR.Domain.Interfaces;
public interface ILocationService
{
    Task<IEnumerable<LocationModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<LocationDetailModel?> GetOneAsync(int LocationId,CancellationToken cancellationToken);
    Task<LocationModel?> CreateOneAsync(LocationCreateModel createModel, CancellationToken cancellationToken);
    Task<LocationModel?> UpdateOneAsync(LocationUpdateModel updateModel, CancellationToken cancellationToken);
    Task<bool> DeleteOneAsync(int LocationId,CancellationToken cancelToken);
}
