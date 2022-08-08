using HR.Domain.Models.CountryModels;

namespace HR.Domain.Models.RegionModels;
public record RegionDetailModel(int RegionId,string RegionName, CountryModel[] Countries);


