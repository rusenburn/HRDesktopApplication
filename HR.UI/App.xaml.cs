using HR.Application.Commands;
using HR.Application.Commands.HttpCommands;
using HR.Application.Services;
using HR.Application.Stores;
using HR.Application.ViewModels.AccountViewModels;
using HR.Application.ViewModels.HomeViewModels;
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
                services.AddViewModelAndExtras<AccountRegisterViewModel>();
                services.AddViewModelAndExtras<HomeIndexViewModel>();

                // services
                services.AddHttpClient<IAccountService, AccountHttpService>();
                services.AddFactory<IAccountService, AccountHttpService>();

                // Non Navigation Commands
                services.AddSingleton<AccountRegisterCommand>();

                // Stores
                services.AddSingleton<INavigationStore, NavigationStore>();
                services.AddSingleton<IAccountStore, AccountStore>();
                    

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
