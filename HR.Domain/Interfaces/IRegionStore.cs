using HR.Domain.StoreModels.RegionStoreModels;
using System.Collections.ObjectModel;

namespace HR.Domain.Interfaces;
public interface IRegionStore
{
    bool IsLoading { get; set; }
    event Action IsLoadingChanged;
    ObservableCollection<RegionStoreModel> AllRegions { get; }
}
