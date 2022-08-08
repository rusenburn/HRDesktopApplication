using HR.Application.Commands;
using HR.Application.ViewModels.AccountViewModels;
using HR.Application.ViewModels.HomeViewModels;
using HR.Application.ViewModels.RegionViewModels;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace HR.Application.ViewModels.SharedComponentsViewModels;
public class NavbarComponentViewModel : ViewModelBase,IViewModel
{
    private bool _isDisposed;

    private readonly IAccountStore _accountStore;

    public ICommand HomeIndexNavigationCommand { get; }
    public ICommand AccountRegisterNavigationCommand { get; }
    public ICommand AccountLoginNavigationCommand { get; }
    public ICommand RegionIndexNavigationCommand { get; }

    public NavbarComponentViewModel(IAccountStore accountStore,
            INavigationCommand<HomeIndexViewModel> homeIndexNavigationCommand,
            INavigationCommand<AccountRegisterViewModel> accountRegisterNavigationCommand,
            INavigationCommand<AccountLoginViewModel> accountLoginNavigationCommand,
            INavigationCommand<RegionIndexViewModel> regionIndexNavigationCommand)
    {
        _accountStore = accountStore;
        HomeIndexNavigationCommand = homeIndexNavigationCommand;
        AccountRegisterNavigationCommand = accountRegisterNavigationCommand;
        AccountLoginNavigationCommand = accountLoginNavigationCommand;
        RegionIndexNavigationCommand = regionIndexNavigationCommand;
    }

    protected override void Dispose(bool disposing)
    {
        if(!_isDisposed)
        {
            if(disposing)
            {

            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }
}
