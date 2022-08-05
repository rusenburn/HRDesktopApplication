using HR.Domain.Interfaces;
using System.ComponentModel;

namespace HR.Domain.Abstracts;
public class StoreModelBase : IStoreModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
