using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.CountryStoreModels;
public class CountryStoreModel : StoreModelBase
{
    private int _countryId;
    private string _countryName = null!;
    private int _regionId;

    public int CountryId
    {
        get => _countryId;
        set { OnPropertyChanged(ref _countryId, value); }
    }
    public string CountryName
    {
        get => _countryName;
        set { OnPropertyChanged(ref _countryName, value); }
    }

    public int RegionId
    {
        get { return _regionId; }
        set { OnPropertyChanged(ref _regionId, value); }
    }

    public CountryStoreModel(int countryId, string countryName, int regionId)
    {
        CountryId = countryId;
        CountryName = countryName;
        RegionId = regionId;
    }
}
