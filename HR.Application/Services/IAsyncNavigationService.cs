using HR.Domain.Interfaces;

namespace HR.Application.Services;
public interface IAsyncNavigationService<TViewModel> where TViewModel : IViewModel
{
    Task NavigateAsync();
}
