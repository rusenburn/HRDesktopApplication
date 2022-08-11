using HR.Application.Commands;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models.CountryModels;
using HR.Domain.StoreModels.CountryStoreModels;
using HR.Domain.StoreModels.RegionStoreModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace HR.Application.ViewModels.CountryViewModels;
public class CountryUpdateComponentViewModel : ViewModelBase
{
    private readonly ICountryStore _countryStore;
    private readonly ICountryService _countryService;
    private readonly IRegionStore _regionStore;
    private bool _isDisposedValue;

    private CancellationTokenSource _tokenSource = new();

    public ObservableCollection<RegionStoreModel> Regions => _regionStore.AllRegions;

    public CountryUpdateStoreModel CountryUpdate => _countryStore.CountryUpdate;

    public RegionStoreModel? SelectedRegion
    {
        get => _regionStore.AllRegions.FirstOrDefault(r => r.RegionId == _countryStore.CountryUpdate.RegionId);
        set => SetSelectedRegion(value);
    }

    public ICommand UpdateCommand { get; }
    public ICommand CancelCommand { get; }

    public CountryUpdateComponentViewModel(ICountryStore countryStore, ICountryService countryService, IRegionStore regionStore)
    {
        _countryStore = countryStore;
        _countryService = countryService;
        _regionStore = regionStore;

        UpdateCommand = new AsyncRelayCommand(UpdateCountryAsync);
        CancelCommand = new RelayCommand(Cancel);

        _countryStore.CountryUpdate.PropertyChanged += CountryUpdate_PropertyChanged;
        _regionStore.AllRegions.CollectionChanged += AllRegions_CollectionChanged;
    }

    private void AllRegions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SelectedRegion));
    }

    private void CountryUpdate_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CountryUpdate.RegionId))
        {
            OnPropertyChanged(nameof(SelectedRegion));
        }
    }

    private async Task UpdateCountryAsync(object? arg)
    {
        CountryUpdateModel updateModel = new CountryUpdateModel(
            CountryId: CountryUpdate.CountryId,
            CountryName: CountryUpdate.CountryName,
            RegionId: CountryUpdate.RegionId
            );

        var cancellationToken = _tokenSource.Token;

        try
        {
            var result = await _countryService.UpdateOneAsync(updateModel, cancellationToken);
            if (result is not null)
            {
                var country = _countryStore.AllCountries.FirstOrDefault(c => c.CountryId == result.CountryId);
                if (country is not null)
                {
                    country.RegionId = result.RegionId;
                    country.CountryName = result.CountryName;
                }
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

    private void Cancel(object? obj)
    {
        _countryStore.IsCountryUpdateEnabled = false;
        _tokenSource.Cancel();
        _tokenSource.TryReset();
    }

    private void SetSelectedRegion(RegionStoreModel? value)
    {
        if (value is null || _countryStore.CountryUpdate.RegionId == value.RegionId)
            return;
        _countryStore.CountryUpdate.RegionId = value.RegionId;
        OnPropertyChanged(nameof(SelectedRegion));
    }
    protected override void Dispose(bool disposing)
    {
        if (!_isDisposedValue)
        {
            if (disposing)
            {
                _tokenSource.Dispose();
                _countryStore.CountryUpdate.PropertyChanged -= CountryUpdate_PropertyChanged;
                _regionStore.AllRegions.CollectionChanged -= AllRegions_CollectionChanged;
            }
            _isDisposedValue = true;
        }

        base.Dispose(disposing);
    }
}
