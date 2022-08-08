using HR.Domain.Models.RegionModels;

namespace HR.Domain.Interfaces;
public interface IRegionService
{
    Task<IEnumerable<RegionModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<RegionDetailModel?> GetOneAsync(int regionId,CancellationToken cancellationToken);
    Task<RegionDetailModel?> CreateOneAsync(RegionCreateModel model,CancellationToken cancellationToken);
    Task<RegionDetailModel?> UpdateOneAsync(RegionUpdateModel model,CancellationToken cancellationToken);
    Task<bool> DeleteOneAsync(int regionId, CancellationToken cancellationToken);
}
