using HR.Domain.Models.CountryModels;

namespace HR.Domain.Models.LocationModels;
public record LocationDetailModel(int LocationId,string StreetAddress,string City,string? PostalCode,string StateProvince,int CountryId,CountryModel Country);


