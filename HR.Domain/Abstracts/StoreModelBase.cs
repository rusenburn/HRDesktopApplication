using HR.Domain.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HR.Domain.Abstracts;
public class StoreModelBase : IStoreModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected virtual bool OnPropertyChanged<T>(ref T property,T value, [CallerMemberName]string propertyName="")
    {
        if (EqualityComparer<T>.Default.Equals(property, value)) return false;
        property = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
