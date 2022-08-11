namespace HR.Application.Commands;
public class RelayCommand : CommandBase
{
    private readonly Action<object?> _action;
    private bool _isExecuting;
    public RelayCommand(Action<object?> action)
    {
        _action = action;
    }

    public override void Execute(object? parameter)
    {
        _isExecuting = true;
        try
        {
            _action(parameter);
        }
        finally
        {
            _isExecuting = false;
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !_isExecuting && base.CanExecute(parameter);
    }
}
