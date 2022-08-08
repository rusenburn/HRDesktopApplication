using System.ComponentModel;

namespace HR.Domain.Interfaces;
public interface IViewModel : INotifyPropertyChanged, IDisposable{
    Task OnInitializedAsync();
}
