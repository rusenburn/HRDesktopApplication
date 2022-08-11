using HR.Domain.Abstracts;

namespace HR.Domain.StoreModels.RegionStoreModels;
public class RegionUpdateStoreModel : StoreModelBase
{
    private int _regionId;
    private string? _regionName;
    public int RegionId
    {
        get => _regionId;
        set { OnPropertyChanged(ref _regionId, value); }
    }
    public string RegionName
    {
        get => _regionName!;
        set { OnPropertyChanged(ref _regionName, value); }
    }

    public RegionUpdateStoreModel(int regionId, string regionName)
    {
        RegionId = regionId;
        RegionName = regionName;
    }




}
