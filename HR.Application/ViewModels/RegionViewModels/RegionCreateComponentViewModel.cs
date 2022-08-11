using HR.Application.Commands;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models.RegionModels;
using HR.Domain.StoreModels.RegionStoreModels;
using System.Windows.Input;

namespace HR.Application.ViewModels.RegionViewModels;
public class RegionCreateComponentViewModel : ViewModelBase
{
    private readonly IRegionStore _regionStore;
    private readonly IRegionService _regionService;
    private bool _disposed;

    public RegionCreateStoreModel RegionCreate => _regionStore.RegionCreateModel;
    private CancellationTokenSource _tokenSource;
    public ICommand CancelCommand { get; }
    public ICommand CreateCommand { get; }
    public RegionCreateComponentViewModel(IRegionStore regionStore, IRegionService regionService)
    {
        _regionStore = regionStore;
        _tokenSource = new CancellationTokenSource();
        CancelCommand = new RelayCommand((p) => { _tokenSource.Cancel(); });
        CreateCommand = new AsyncRelayCommand(CreateRegionAsync);
        _regionService = regionService;
    }

    private async Task CreateRegionAsync(object? parameter)
    {
        RegionCreateModel createModel = new(RegionCreate.RegionName);
        var token = _tokenSource.Token;
        try
        {
            var res = await _regionService.CreateOneAsync(createModel, token);
            if (res is not null)
            {
                _regionStore.AllRegions.Add(new(regionId: res.RegionId, regionName: res.RegionName));
            }
        }
        catch (OperationCanceledException e)
        {

        }
        finally
        {
            _tokenSource.TryReset();
        }

    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _tokenSource.Dispose();
            }
            _disposed = true;
        }
        base.Dispose(disposing);
    }
}
