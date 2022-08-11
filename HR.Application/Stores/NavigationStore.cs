using HR.Domain.Interfaces;

namespace HR.Application.Stores;
public class NavigationStore : INavigationStore
{
    private IViewModel? _currentViewModel = null;
    public IViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            var temp = _currentViewModel;
            _currentViewModel = value;
            OnCurrentViewModelChanged();
            temp?.Dispose();
        }
    }

    public event Action CurrentViewModelChanged = () => { };

    public void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged.Invoke();
    }
}
