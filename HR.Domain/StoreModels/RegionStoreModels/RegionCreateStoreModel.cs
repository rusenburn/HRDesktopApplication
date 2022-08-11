using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.RegionStoreModels;
public class RegionCreateStoreModel : StoreModelBase
{
    private string _regionName = null!;

    public string RegionName
    {
        get { return _regionName; }
        set { OnPropertyChanged(ref _regionName, value); }
    }

    public RegionCreateStoreModel(string regionName)
    {
        RegionName = regionName;
    }
}
