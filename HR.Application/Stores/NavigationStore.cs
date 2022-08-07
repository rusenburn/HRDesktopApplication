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
            _currentViewModel?.Dispose();
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public event Action CurrentViewModelChanged = () => { };

    public void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged.Invoke();
    }
}
