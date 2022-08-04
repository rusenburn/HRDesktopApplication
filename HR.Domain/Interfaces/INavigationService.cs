using HR.Domain.Abstracts;

namespace HR.Domain.Interfaces;
public interface INavigationService<TViewModel> where TViewModel : IViewModel
{
    void Navigate();
}
