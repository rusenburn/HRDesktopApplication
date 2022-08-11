using HR.Application.Commands;
using HR.Application.ViewModels.AccountViewModels;
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
    private readonly IRegionService _regionService;
    private bool _isDisposed;
    #endregion


    #region Public Properties
    public RegionCreateComponentViewModel RegionCreateComponent { get; }
    public RegionUpdateComponentViewModel RegionUpdateComponent { get; }
    public ObservableCollection<RegionStoreModel> Regions => _regionStore.AllRegions;
    public bool IsLoading => _regionStore.IsLoading;
    public bool IsRegionUpdateEnabled => _regionStore.IsRegionUpdateEnabled;
    public ICommand EditRegionCommand { get; }
    #endregion


    #region Constructors
    public RegionIndexViewModel(IRegionStore regionStore,
            IRegionService regionService,
            IFactory<RegionCreateComponentViewModel> regionCreateComponentFactory,
            IFactory<RegionUpdateComponentViewModel> regionUpdateComponentFactory)
    {
        
        _regionStore = regionStore ?? throw new ArgumentNullException(nameof(regionStore));
        _regionService = regionService;
        RegionCreateComponent = regionCreateComponentFactory.Create();
        RegionUpdateComponent = regionUpdateComponentFactory.Create();
        EditRegionCommand = new RelayCommand(EditRegion);
        _regionStore.IsLoadingChanged += OnIsLoadingChanged;
        _regionStore.IsRegionUpdateEnabledChanged += OnIsRegionUpdateEnabledChanged;
    }


    #endregion



    public override async Task OnInitializedAsync()
    {
        _regionStore.IsLoading = true;
        _regionStore.AllRegions.Clear();

#if DEBUG
        await Task.Delay(500);
#endif

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
        await RegionCreateComponent.OnInitializedAsync();
        await RegionUpdateComponent.OnInitializedAsync();
    }


    #region Private Functions
    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _regionStore.IsLoadingChanged -= OnIsLoadingChanged;
                _regionStore.IsRegionUpdateEnabledChanged -= OnIsRegionUpdateEnabledChanged;
            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }

    private void EditRegion(object? obj)
    {
        if (obj is not RegionStoreModel region) return;

        _regionStore.RegionUpdate.RegionId = region.RegionId;
        _regionStore.RegionUpdate.RegionName = region.RegionName;
        _regionStore.IsRegionUpdateEnabled = true;
    }

    private void OnIsLoadingChanged()
    {
        OnPropertyChanged(nameof(IsLoading));
    }

    private void OnIsRegionUpdateEnabledChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsRegionUpdateEnabled));
    }
    #endregion





}
