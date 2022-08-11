using HR.Application.Commands;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models.RegionModels;
using HR.Domain.StoreModels.RegionStoreModels;
using System.Windows.Input;

namespace HR.Application.ViewModels.RegionViewModels;
public class RegionUpdateComponentViewModel : ViewModelBase
{
    private readonly IRegionStore _regionStore;
    private readonly IRegionService _regionService;
    private CancellationTokenSource _tokenSource;
    private bool _isDisposed;
    public RegionUpdateStoreModel RegionUpdate => _regionStore.RegionUpdate;
    public ICommand UpdateCommand { get; }
    public ICommand CancelCommand { get; }

    public RegionUpdateComponentViewModel(IRegionStore regionStore, IRegionService regionService)
    {
        _regionStore = regionStore;
        _regionService = regionService;
        _tokenSource = new CancellationTokenSource();
        CancelCommand = new RelayCommand(Cancel);
        UpdateCommand = new AsyncRelayCommand(UpdateRegionAsync);
    }
    private void Cancel(object? obj)
    {
        _tokenSource.Cancel();
        _regionStore.IsRegionUpdateEnabled = false;
    }
    private async Task UpdateRegionAsync(object? arg)
    {
        RegionUpdateModel regionUpdate = new(
            RegionId: RegionUpdate.RegionId,
            RegionName: RegionUpdate.RegionName);
        var token = _tokenSource.Token;
        try
        {
            var res = await _regionService.UpdateOneAsync(regionUpdate, token);
            if (res is not null)
            {
                var regionInList = _regionStore.AllRegions.FirstOrDefault(x => x.RegionId == res.RegionId);
                if (regionInList is not null)
                {
                    regionInList.RegionName = res.RegionName;
                }
            }
        }
        catch (OperationCanceledException ex)
        {
        }
        finally
        {
            _tokenSource.TryReset();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _tokenSource.Dispose();
            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
}
