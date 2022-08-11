using HR.Application.Commands;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models.CountryModels;
using HR.Domain.StoreModels.CountryStoreModels;
using HR.Domain.StoreModels.RegionStoreModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HR.Application.ViewModels.CountryViewModels;
public class CountryCreateComponentViewModel : ViewModelBase
{
    private readonly ICountryStore _countryStore;
    private readonly ICountryService _countryService;
    private readonly IRegionStore _regionStore;
    private CancellationTokenSource _tokenSource = new CancellationTokenSource();
    private bool _isDisposed;

    public ObservableCollection<RegionStoreModel> Regions => _regionStore.AllRegions;

    public CountryCreateStoreModel CountryCreate => _countryStore.CountryCreate;

    public RegionStoreModel? SelectedRegion
    {
        get => _regionStore.AllRegions.FirstOrDefault(x => x.RegionId == _countryStore.CountryCreate.RegionId);
        set => SetSelectedRegion(value);
    }
    public ICommand CreateCommand { get; }
    public ICommand CancelCommand { get; }

    public CountryCreateComponentViewModel(ICountryStore countryStore, ICountryService countryService, IRegionStore regionStore)
    {
        _countryStore = countryStore;
        _countryService = countryService;
        _regionStore = regionStore;
        CreateCommand = new AsyncRelayCommand(CreateCountryAsync);
        CancelCommand = new RelayCommand(Cancel);
    }



    private void SetSelectedRegion(RegionStoreModel? value)
    {
        if (value is null || _countryStore.CountryCreate.RegionId == value.RegionId)
            return;
        _countryStore.CountryCreate.RegionId = value.RegionId;
        OnPropertyChanged(nameof(SelectedRegion));
    }

    private async Task CreateCountryAsync(object? arg)
    {
        CountryCreateModel createModel = new CountryCreateModel(
            CountryName: CountryCreate.CountryName,
            RegionId: CountryCreate.RegionId
            );

        var cancellationtoken = _tokenSource.Token;
        try
        {
            var result = await _countryService.CreateOneAsync(createModel, cancellationtoken);
            if (result is not null)
            {
                // Add country to Collection in store
                _countryStore.AllCountries.Add(new(
                    countryId: result.CountryId,
                    countryName: result.CountryName,
                    regionId: result.RegionId));
                
                // reset form
                CountryCreate.CountryName = "";
                CountryCreate.RegionId = 0;
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
        _tokenSource.Cancel();
        _tokenSource.TryReset();
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
