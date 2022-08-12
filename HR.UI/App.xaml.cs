﻿using HR.Application.Commands;
using HR.Application.Commands.HttpCommands;
using HR.Application.Services;
using HR.Application.Stores;
using HR.Application.ViewModels.AccountViewModels;
using HR.Application.ViewModels.CountryViewModels;
using HR.Application.ViewModels.HomeViewModels;
using HR.Application.ViewModels.RegionViewModels;
using HR.Application.ViewModels.SharedComponentsViewModels;
using HR.DAL.HttpServices;
using HR.Domain.Abstracts;
using HR.Domain.Interfaces;
using HR.UI.Extensions;
using HR.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace HR.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;

        public App()
        {
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices((hostbuilder, services) =>
            {
                services.AddViewModelAndExtras<AccountRegisterViewModel>(layout: false);
                services.AddViewModelAndExtras<AccountLoginViewModel>(layout: false);
                services.AddViewModelAndExtras<HomeIndexViewModel>();
                services.AddViewModelAndExtras<NavbarComponentViewModel>();
                services.AddViewModelAndExtras<RegionIndexViewModel>();
                services.AddViewModelAndExtras<RegionCreateComponentViewModel>();
                services.AddViewModelAndExtras<RegionUpdateComponentViewModel>();
                services.AddViewModelAndExtras<CountryIndexViewModel>();
                services.AddViewModelAndExtras<CountryCreateComponentViewModel>();
                services.AddViewModelAndExtras<CountryUpdateComponentViewModel>();

                // services
                services.AddHttpClient<IAccountService, AccountHttpService>();
                services.AddFactory<IAccountService, AccountHttpService>();

                services.AddHttpClient<IRegionService, RegionHttpService>();
                services.AddFactory<IRegionService, RegionHttpService>();

                services.AddHttpClient<ICountryService, CountryHttpService>();
                services.AddFactory<ICountryService, CountryHttpService>();

                services.AddHttpClient<ILocationService, LocationHttpService>();
                services.AddFactory<ILocationService, LocationHttpService>();

                // Non Navigation Commands
                services.AddSingleton<AccountRegisterCommand>();
                services.AddSingleton<AccountLoginCommand>();

                // Stores
                services.AddSingleton<INavigationStore, NavigationStore>();
                services.AddSingleton<IAccountStore, AccountStore>();
                services.AddSingleton<IAuthorizationStore, AuthorizationStore>();
                services.AddSingleton<IRegionStore, RegionStore>();
                services.AddSingleton<ICountryStore, CountryStore>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();
            });
            _host = builder.Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            var accountRegisterNavigation = _host.Services.GetRequiredService<INavigationService<AccountRegisterViewModel>>();
            accountRegisterNavigation.Navigate();
            MainWindow = mainWindow;
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
