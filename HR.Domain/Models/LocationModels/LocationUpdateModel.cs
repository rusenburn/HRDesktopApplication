namespace HR.Domain.Models.LocationModels;
public record LocationUpdateModel(int LocationId,string StreetAddress, string City, string? PostalCode, string StateProvince, int CountryId);
