namespace HR.Application.Commands;
public class RelayCommand : CommandBase
{
    private readonly Action<object?> _action;
    public RelayCommand(Action<object?> action)
    {
        _action = action;
    }

    public override void Execute(object? parameter)
    {
        _action(parameter);
    }
}
