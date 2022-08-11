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
        set
        {
            _isLoading = value;
            OnIsLoadingChanged();
        }
    }

    private bool _isRegionUpdateEnabled;

    public bool IsRegionUpdateEnabled
    {
        get => _isRegionUpdateEnabled;
        set
        {
            _isRegionUpdateEnabled = value;
            OnIsRegionUpdateEnabledChanged();
        }
    }

    public event Action IsLoadingChanged = () => { };
    public event EventHandler RegionCreateModelChanged = (s, e) => { };
    public event EventHandler IsRegionUpdateEnabledChanged = (s, e) => { };
    public ObservableCollection<RegionStoreModel> AllRegions { get; } = new();

    public RegionCreateStoreModel RegionCreateModel { get; } = new("");
    public RegionUpdateStoreModel RegionUpdate { get; } = new(0, "");
    protected void OnIsLoadingChanged()
    {
        IsLoadingChanged.Invoke();
    }
    protected void OnIsRegionUpdateEnabledChanged()
    {
        IsRegionUpdateEnabledChanged.Invoke(this, EventArgs.Empty);
    }

}
