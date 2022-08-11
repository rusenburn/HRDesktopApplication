using HR.Domain.StoreModels.CountryStoreModels;
using System.Collections.ObjectModel;

namespace HR.Domain.Interfaces;
public interface ICountryStore
{
    bool IsLoading { get; set; }
    ObservableCollection<CountryStoreModel> AllCountries { get; set; }
    CountryUpdateStoreModel CountryUpdate { get; }
    bool IsCountryUpdateEnabled { get; set; }

    event EventHandler IsLoadingChangedHandler;
    event EventHandler IsCountryUpdateEnabledChanged;
}
