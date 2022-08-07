using HR.Domain.Interfaces;

namespace HR.Application.Services;
public class NavigationService<TViewModel> : INavigationService<TViewModel> 
    where TViewModel : IViewModel
{
    private readonly IFactory<TViewModel> _viewModelFactory;
    private readonly INavigationStore _navigationStore;

    public NavigationService(IFactory<TViewModel> viewModelFactory,INavigationStore navigationStore)
    {
        _viewModelFactory = viewModelFactory;
        _navigationStore = navigationStore;
    }

    public void Navigate()
    {
         var viewModel =   _viewModelFactory.Create();
        _navigationStore.CurrentViewModel = viewModel;
    }
}
