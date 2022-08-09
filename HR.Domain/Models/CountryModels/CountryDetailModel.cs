using HR.Domain.Models.RegionModels;

namespace HR.Domain.Models.CountryModels;
public record CountryDetailModel(int CountryId,string CountryName,int RegionId,RegionModel Region);

