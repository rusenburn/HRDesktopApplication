using HR.Application.Services;
using HR.Application.Stores;
using HR.Application.ViewModels.AccountViewModels;
using HR.Application.ViewModels.HomeViewModels;
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
                //services.AddViewModels();
                services.AddViewModelAndExtras<AccountRegisterViewModel>();
                services.AddViewModelAndExtras<HomeIndexViewModel>();
                //services.AddTransient<AccountRegisterViewModel>();
                //services.AddSingleton<Func<AccountRegisterViewModel>>(s => () => s.GetRequiredService<AccountRegisterViewModel>());
                //services.AddSingleton<IFactory<AccountRegisterViewModel>,FactoryBase<AccountRegisterViewModel>>();
                //services.AddSingleton<INavigationService<AccountRegisterViewModel>,NavigationService<AccountRegisterViewModel>>();

                services.AddSingleton<INavigationStore, NavigationStore>();
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
