using HR.Application.Services;
using HR.Domain.Interfaces;

namespace HR.Application.Commands;
public class AsyncNavigationCommand<T> : AsyncCommandBase, INavigationCommand<T>
    where T : IViewModel
{
    private readonly IAsyncNavigationService<T> _navigationService;

    public AsyncNavigationCommand(IAsyncNavigationService<T> navigationService)
    {
        _navigationService = navigationService;
    }

    protected override async Task ExecuteAsync(object? parameter)
    {
        await _navigationService.NavigateAsync();
    }
}
