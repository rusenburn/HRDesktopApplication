using HR.Application.Commands;
using HR.Application.ViewModels.AccountViewModels;
using HR.Application.ViewModels.CountryViewModels;
using HR.Application.ViewModels.HomeViewModels;
using HR.Application.ViewModels.RegionViewModels;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.Domain.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace HR.Application.ViewModels.SharedComponentsViewModels;
public class NavbarComponentViewModel : ViewModelBase, IViewModel
{
    private bool _isDisposed;

    private readonly IAccountStore _accountStore;
    private readonly IAuthorizationStore _authorizationStore;

    public ICommand HomeIndexNavigationCommand { get; }
    public ICommand AccountRegisterNavigationCommand { get; }
    public ICommand AccountLoginNavigationCommand { get; }
    public ICommand RegionIndexNavigationCommand { get; }
    public ICommand CountryIndexNavigationCommand { get; }
    public ICommand LogoutCommand { get; }
    public bool IsLoggedIn => !_authorizationStore.IsTokenExpired;

    public NavbarComponentViewModel(IAccountStore accountStore,
            INavigationCommand<HomeIndexViewModel> homeIndexNavigationCommand,
            INavigationCommand<AccountRegisterViewModel> accountRegisterNavigationCommand,
            INavigationCommand<AccountLoginViewModel> accountLoginNavigationCommand,
            INavigationCommand<RegionIndexViewModel> regionIndexNavigationCommand,
            INavigationCommand<CountryIndexViewModel> countryIndexNavigationCommand,
            IAuthorizationStore authorizationStore)
    {
        _accountStore = accountStore;
        HomeIndexNavigationCommand = homeIndexNavigationCommand;
        AccountRegisterNavigationCommand = accountRegisterNavigationCommand;
        AccountLoginNavigationCommand = accountLoginNavigationCommand;
        RegionIndexNavigationCommand = regionIndexNavigationCommand;
        CountryIndexNavigationCommand = countryIndexNavigationCommand;
        _authorizationStore = authorizationStore;

        LogoutCommand = new RelayCommand(Logout);
        _authorizationStore.AuthorizationChanged += OnAuthorizationChanged;
    }

    private void OnAuthorizationChanged(AccountInformationModel obj)
    {
        OnPropertyChanged(nameof(IsLoggedIn));
    }

    private void Logout(object? obj)
    {
        _authorizationStore.Logout();
        HomeIndexNavigationCommand.Execute(null);
    }

    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {

            }
            _isDisposed = true;
        }
        base.Dispose(disposing);
    }


}
