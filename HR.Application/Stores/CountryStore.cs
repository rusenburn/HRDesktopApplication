using HR.Domain.Interfaces;
using HR.Domain.StoreModels.CountryStoreModels;
using System.Collections.ObjectModel;

namespace HR.Application.Stores;
public class CountryStore : ICountryStore
{
    private bool _isLoading;

    public bool IsLoading
    {
        get { return _isLoading; }
        set
        {
            if (_isLoading == value) return;
            _isLoading = value;
            OnIsLoadingChanged();
        }
    }

    private bool _isCountryUpdateEnabled;

    public bool IsCountryUpdateEnabled
    {
        get => _isCountryUpdateEnabled;
        set
        {
            if (_isCountryUpdateEnabled == value) return;
            _isCountryUpdateEnabled = value;
            OnIsCountryUpdateEnabledChanged();
        }
    }

    public CountryUpdateStoreModel CountryUpdate { get; } = new CountryUpdateStoreModel(0, "", 0);
    public CountryCreateStoreModel CountryCreate { get; } = new CountryCreateStoreModel("", 0);
    public ObservableCollection<CountryStoreModel> AllCountries { get; set; } = new ObservableCollection<CountryStoreModel>();

    public event EventHandler IsLoadingChangedHandler = (s, e) => { };
    public event EventHandler IsCountryUpdateEnabledChanged = (s, e) => { };

    protected virtual void OnIsLoadingChanged()
    {
        IsLoadingChangedHandler.Invoke(this, EventArgs.Empty);
    }
    protected virtual void OnIsCountryUpdateEnabledChanged()
    {
        IsCountryUpdateEnabledChanged.Invoke(this, EventArgs.Empty);
    }
}
