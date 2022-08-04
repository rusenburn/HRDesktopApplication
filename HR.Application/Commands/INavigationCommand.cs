using HR.Domain.Interfaces;
using System.Windows.Input;

namespace HR.Application.Commands;
public interface INavigationCommand<TViewModel> : ICommand
    where TViewModel : IViewModel
{
}
