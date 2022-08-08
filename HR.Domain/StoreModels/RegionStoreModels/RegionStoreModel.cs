using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.RegionStoreModels;
public class RegionStoreModel : StoreModelBase
{
    private int _regionId;

    public int RegionId
    {
        get => _regionId;
        set { OnPropertyChanged(ref _regionId, value); }
    }
    private string? _regionName;

    public string RegionName
    {
        get { return _regionName!; }
        set { OnPropertyChanged(ref _regionName, value); }
    }

    public RegionStoreModel(int regionId, string regionName)
    {
        RegionId = regionId;
        RegionName = regionName;
    }
}


