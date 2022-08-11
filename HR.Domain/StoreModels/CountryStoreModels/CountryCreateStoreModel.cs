using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.CountryStoreModels;
public class CountryCreateStoreModel : StoreModelBase
{

    private string _countryName = null!;
    private int _regionId;

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

    public CountryCreateStoreModel(string countryName, int regionId)
    {
        CountryName = countryName;
        RegionId = regionId;
    }

}
