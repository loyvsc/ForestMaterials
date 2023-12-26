using System.Windows;
using System.Windows.Controls;

namespace BuildMaterials.Views
{
    public partial class LoginView : Window
    {
        private readonly ViewModels.LoginViewModel viewModel;
        public LoginView()
        {
            viewModel = new ViewModels.LoginViewModel(this);
            InitializeComponent();
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.EnteredPassword = ((PasswordBox)sender).Password;
        }
    }
}