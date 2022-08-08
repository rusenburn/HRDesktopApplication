using HR.Domain.Abstracts;
using HR.Domain.Interfaces;

namespace HR.Application.ViewModels.SharedComponentsViewModels;
public class LayoutComponentViewModel : ViewModelBase, IViewModel
{
    private bool _isDisposed;

    public LayoutComponentViewModel(IViewModel contentViewModel, NavbarComponentViewModel navbarComponentViewModel)
    {
        ContentViewModel = contentViewModel;
        NavbarComponentViewModel = navbarComponentViewModel;
    }

    public IViewModel? ContentViewModel { get; }
    public IViewModel? NavbarComponentViewModel { get; }
    //public IViewModel? NavbarComponentViewModel { get; }


    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                ContentViewModel?.Dispose();
                NavbarComponentViewModel?.Dispose();
            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
}
