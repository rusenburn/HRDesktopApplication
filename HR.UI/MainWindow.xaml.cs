using HR.UI.ViewModels;
using System.Windows;

namespace HR.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            DataContext = mainViewModel;
            InitializeComponent();
        }
    }
}
