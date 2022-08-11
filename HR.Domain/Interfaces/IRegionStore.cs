using HR.Domain.StoreModels.RegionStoreModels;
using System.Collections.ObjectModel;

namespace HR.Domain.Interfaces;
public interface IRegionStore
{
    bool IsLoading { get; set; }
    event Action IsLoadingChanged;
    bool IsRegionUpdateEnabled { get; set; }
    event EventHandler IsRegionUpdateEnabledChanged;
    ObservableCollection<RegionStoreModel> AllRegions { get; }
    RegionCreateStoreModel RegionCreateModel { get; }
    RegionUpdateStoreModel RegionUpdate { get; }
}
