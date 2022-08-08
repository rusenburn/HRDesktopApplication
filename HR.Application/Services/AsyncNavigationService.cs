//using HR.Domain.Interfaces;

//namespace HR.Application.Services;
//public class AsyncNavigationService<TViewModel> : IAsyncNavigationService<TViewModel> where TViewModel : IViewModel
//{
//    private readonly IFactory<TViewModel> _viewModelFactory;
//    private readonly INavigationStore _navigationStore;

//    public AsyncNavigationService(IFactory<TViewModel> viewModelFactory,INavigationStore navigationStore)
//    {
//        _viewModelFactory = viewModelFactory;
//        _navigationStore = navigationStore;
//    }
//    public async Task NavigateAsync()
//    {
//        var viewModel = _viewModelFactory.Create();
//        _navigationStore.CurrentViewModel = viewModel;
//        await viewModel.OnInitializedAsync();
//    }
//}
