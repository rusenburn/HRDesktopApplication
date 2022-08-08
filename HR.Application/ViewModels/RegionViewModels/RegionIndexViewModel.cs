using HR.Application.Commands;
using HR.Application.ViewModels.AccountViewModels;
using HR.Application.ViewModels.HomeViewModels;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.StoreModels.RegionStoreModels;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace HR.Application.ViewModels.RegionViewModels;
public class RegionIndexViewModel : ViewModelBase
{
    #region Private Members
    private readonly IRegionStore _regionStore;
    private readonly IRegionService _regionService;
    private bool _isDisposed;
    #endregion


    #region Public Properties
    public ObservableCollection<RegionStoreModel> Regions => _regionStore.AllRegions;
    public bool IsLoading => _regionStore.IsLoading;
    public ICommand RegionCreateNavigationCommand { get; }
    #endregion


    #region Constructors
    public RegionIndexViewModel(IRegionStore regionStore, 
            INavigationCommand<HomeIndexViewModel> regionCreateNavigationCommand,
            INavigationCommand<AccountLoginViewModel> accountLoginNavigationCommand,
            IRegionService regionService)
    {
        ArgumentNullException.ThrowIfNull(accountLoginNavigationCommand);
        _regionStore = regionStore ?? throw new ArgumentNullException(nameof(regionStore));
        RegionCreateNavigationCommand = regionCreateNavigationCommand ?? throw new ArgumentNullException(nameof(regionCreateNavigationCommand));
        _regionService = regionService;
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

    public override async Task OnInitializedAsync()
    {
        _regionStore.IsLoading = true;
        _regionStore.AllRegions.Clear();
        await Task.Delay(5000);
        try
        {
            using CancellationTokenSource cancellationTokenSource = new();
            var regions = await _regionService.GetAllAsync(cancellationTokenSource.Token);
            foreach (var region in regions)
            {
                _regionStore.AllRegions.Add(new RegionStoreModel(region.RegionId, region.RegionName));
            }
        }
        finally
        {
            _regionStore.IsLoading = false;
        }
        throw new Exception();
    }
    private void OnIsLoadingChanged()
    {
        OnPropertyChanged(nameof(IsLoading));
    }
    #endregion


  

    
}
