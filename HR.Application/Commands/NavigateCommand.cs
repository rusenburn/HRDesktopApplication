using HR.Domain.Interfaces;

namespace HR.Application.Commands;
public class NavigateCommand<TViewModel> : CommandBase, INavigationCommand<TViewModel>
    where TViewModel : IViewModel
{
    private readonly INavigationService<TViewModel> _navigationService;

    public NavigateCommand(INavigationService<TViewModel> navigationService)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object? parameter)
    {
        _navigationService.Navigate();
    }
}
