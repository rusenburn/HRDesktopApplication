using HR.Domain.Interfaces;
using HR.Domain.StoreModels.RegionStoreModels;
using System.Collections.ObjectModel;

namespace HR.Application.Stores;
public class RegionStore : IRegionStore
{
    private bool _isLoading;

    public bool IsLoading
    {
        get { return _isLoading; }
        set { _isLoading = value; }
    }
    public event Action IsLoadingChanged = () => { };

    public ObservableCollection<RegionStoreModel> AllRegions { get; } = new();
}
