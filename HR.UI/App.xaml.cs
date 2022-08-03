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
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();
            });
            _host = builder.Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
