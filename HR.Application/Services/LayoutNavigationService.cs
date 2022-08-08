using HR.Application.ViewModels.SharedComponentsViewModels;
using HR.Domain.Interfaces;

namespace HR.Application.Services;
public class LayoutNavigationService<TViewModel> : INavigationService<TViewModel>, IAsyncNavigationService<TViewModel>
    where TViewModel : IViewModel
{
    private readonly INavigationStore _navigationStore;
    private readonly IFactory<TViewModel> _viewModelFactory;
    private readonly IFactory<NavbarComponentViewModel> _navbarFactory;

    public LayoutNavigationService(INavigationStore navigationStore, IFactory<TViewModel> viewModel, IFactory<NavbarComponentViewModel> navbarFactory)
    {
        _navigationStore = navigationStore;
        _viewModelFactory = viewModel;
        _navbarFactory = navbarFactory;
    }

    public void Navigate()
    {
        _navigationStore.CurrentViewModel = new LayoutComponentViewModel(_viewModelFactory.Create(), _navbarFactory.Create());
    }

    public async Task NavigateAsync()
    {
        var viewModel = _viewModelFactory.Create();
        var navbar = _navbarFactory.Create();
        _navigationStore.CurrentViewModel = new LayoutComponentViewModel(viewModel,navbar);
        await navbar.OnInitializedAsync();
        await viewModel.OnInitializedAsync();
    }
}
