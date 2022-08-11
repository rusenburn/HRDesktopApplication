using HR.Domain.Models.CountryModels;

namespace HR.Domain.Interfaces;
public interface ICountryService
{
    Task<IEnumerable<CountryModel>> GetAllAsync(CountryQueryModel? query, CancellationToken cancellationToken);
    Task<CountryDetailModel?> GetOneAsync(int countryId, CancellationToken cancellationToken);
    Task<CountryModel?> CreateOneAsync(CountryCreateModel createModel, CancellationToken cancellationToken);
    Task<CountryModel?> UpdateOneAsync(CountryUpdateModel updateModel, CancellationToken cancellationToken);
    Task<bool> DeleteOneAsync(int countryId, CancellationToken cancellationToken);
}
