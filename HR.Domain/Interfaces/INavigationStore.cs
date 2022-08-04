namespace HR.Domain.Interfaces;
public interface INavigationStore
{
    event Action CurrentViewModelChanged;
    IViewModel? CurrentViewModel { get; set; }

    // TODO : check if OnCurrentViewModelChanged is needed or not
    void OnCurrentViewModelChanged();
 
}
