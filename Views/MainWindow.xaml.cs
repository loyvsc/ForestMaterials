using BuildMaterials.Models;
using BuildMaterials.ViewModels;
using System.ComponentModel;

namespace BuildMaterials.Views
{
    public partial class MainWindow : FluentWindow
    {
        private readonly MainWindowViewModel mainViewModel;

        public MainWindow(Employee employee)
        {
            InitializeComponent();
            mainViewModel = new MainWindowViewModel(this, employee);
            DataContext = mainViewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            mainViewModel.ExitFromProgramm(e);
            base.OnClosing(e);
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            mainViewModel.OnTabChanged(e);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, System.Windows.RoutedEventArgs args)
        {

        }
    }
}