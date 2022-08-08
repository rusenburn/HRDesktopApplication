using HR.Application.Commands;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.StoreModels.RegionStoreModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HR.Application.ViewModels.RegionViewModels;
public class RegionIndexViewModel : ViewModelBase
{
    #region Private Members
    private readonly IRegionStore _regionStore;
    private bool _isDisposed;
    #endregion


    #region Public Properties
    public ObservableCollection<RegionStoreModel> Regions => _regionStore.AllRegions;
    public bool IsLoading => _regionStore.IsLoading;
    public ICommand RegionCreateNavigationCommand { get; }
    #endregion


    #region Constructors
    public RegionIndexViewModel(IRegionStore regionStore, INavigationCommand<RegionIndexViewModel> regionCreateNavigationCommand)
    {
        _regionStore = regionStore;
        RegionCreateNavigationCommand = regionCreateNavigationCommand;
        _regionStore.IsLoadingChanged += OnIsLoadingChanged;
    }
    #endregion


    #region Private Functions
    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _regionStore.IsLoadingChanged -= OnIsLoadingChanged;
            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }

    public virtual async Task OnInitializeAsync()
    {
        await Task.CompletedTask;
    }
    private void OnIsLoadingChanged()
    {
        OnPropertyChanged(nameof(IsLoading));
    }
    #endregion


  

    
}
