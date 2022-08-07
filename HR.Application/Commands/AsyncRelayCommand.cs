namespace HR.Application.Commands;
public class AsyncRelayCommand : AsyncCommandBase
{
    private readonly Func<object?, Task> _func;

    public AsyncRelayCommand(Func<object?,Task> func)
    {
        _func = func;
    }
    protected override async Task ExecuteAsync(object? parameter)
    {
        await _func(parameter);
    }
}
