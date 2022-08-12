namespace HR.Domain.Models.LocationModels;
public record LocationCreateModel(string StreetAddress, string City, string? PostalCode, string StateProvince, int CountryId);
