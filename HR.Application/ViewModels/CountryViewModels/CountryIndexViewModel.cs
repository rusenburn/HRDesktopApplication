using HR.Application.Commands;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models.CountryModels;
using HR.Domain.StoreModels.CountryStoreModels;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace HR.Application.ViewModels.CountryViewModels;
public class CountryIndexViewModel : ViewModelBase
{
    private bool _isDisposed;
    private readonly ICountryStore _countryStore;
    private readonly ICountryService _countryService;

    public bool IsLoading => _countryStore.IsLoading;
    public bool IsCountryUpdateEnabled => _countryStore.IsCountryUpdateEnabled;
    public ObservableCollection<CountryStoreModel> Countries => _countryStore.AllCountries;
    public ICommand EditCountryCommand { get; }
    public IViewModel CountryCreateComponent { get; }
    public IViewModel CountryUpdateComponent { get; }

    public CountryIndexViewModel(ICountryStore countryStore,
                                 ICountryService countryService,
                                 IFactory<CountryCreateComponentViewModel> countryCreateComponentFactory,
                                 IFactory<CountryUpdateComponentViewModel> countryUpdateComponentFactory)
    {
        _countryStore = countryStore;
        _countryService = countryService;
        EditCountryCommand = new RelayCommand(EditCountry);
        CountryCreateComponent = countryCreateComponentFactory.Create();
        CountryUpdateComponent = countryUpdateComponentFactory.Create();

        _countryStore.IsLoadingChangedHandler += OnIsLoadingChanged;
        _countryStore.IsCountryUpdateEnabledChanged += OnIsCountryUpdateEnabledChanged;
    }

    private void OnIsCountryUpdateEnabledChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsCountryUpdateEnabled));
    }

    public async override Task OnInitializedAsync()
    {
        _countryStore.IsLoading = true;
        _countryStore.AllCountries.Clear();
#if DEBUG
        await Task.Delay(1000);
#endif
        try
        {
            using CancellationTokenSource cancellationTokenSource = new();
            var countries = await _countryService.GetAllAsync(new CountryQueryModel(), cancellationTokenSource.Token);

            foreach (var country in countries)
            {
                _countryStore.AllCountries.Add(new CountryStoreModel(
                    countryId: country.CountryId,
                    countryName: country.CountryName,
                    regionId: country.RegionId));
            }
        }
        finally
        {
            _countryStore.IsLoading = false;
        }

        await CountryCreateComponent.OnInitializedAsync();
        await CountryUpdateComponent.OnInitializedAsync();
    }

    private void OnIsLoadingChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsLoading));

    }

    private void EditCountry(object? obj)
    {
        if (obj is not CountryStoreModel country) return;
        _countryStore.CountryUpdate.CountryId = country.CountryId;
        _countryStore.CountryUpdate.RegionId = country.RegionId;
        _countryStore.CountryUpdate.CountryName = country.CountryName;
        _countryStore.IsCountryUpdateEnabled = true;


    }

    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            if (disposing)
            {
                _countryStore.IsLoadingChangedHandler -= OnIsLoadingChanged;
                _countryStore.IsCountryUpdateEnabledChanged += OnIsCountryUpdateEnabledChanged;
                CountryUpdateComponent.Dispose();
                CountryCreateComponent.Dispose();
            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
}
