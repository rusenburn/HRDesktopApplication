using HR.Domain.Interfaces;
using System.ComponentModel;

namespace HR.Domain.Abstracts;
public abstract class ViewModelBase : IViewModel
{
    private bool _disposedValue;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // dispose managed state (managed objects) here
            }
            // free unmanaged resources (unmanaged objects) and override finalizer here
            // set large fields to null
            _disposedValue = true;
        }
    }

    //// override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    //~ViewModelBase()
    //{
    //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //    Dispose(disposing: false);
    //}

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

