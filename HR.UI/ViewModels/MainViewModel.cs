using HR.Domain.Abstracts;
using HR.Domain.Interfaces;

namespace HR.UI.ViewModels;
public class MainViewModel : ViewModelBase
{
    private bool _isDisposedValue;
    private readonly INavigationStore _navigationStore;
    public IViewModel? CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel(INavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    protected override void Dispose(bool disposing)
    {
        if(!_isDisposedValue)
        {
            if(disposing)
            {
                _navigationStore.CurrentViewModelChanged-=OnCurrentViewModelChanged;
            }
            _isDisposedValue = true;
        }
        base.Dispose(disposing);
    }
}
