using BuildMaterials.Models;
using BuildMaterials.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace BuildMaterials.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainWindowViewModel(this);
            DataContext = mainViewModel;
        }

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
    }
}