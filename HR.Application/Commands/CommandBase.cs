
using System.Windows.Input;

namespace HR.Application.Commands;
public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged = (sender, e) => { };
    public virtual bool CanExecute(object? parameter) => true;
    public abstract void Execute(object? parameter);
    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}
