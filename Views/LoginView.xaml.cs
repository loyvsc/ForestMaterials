namespace BuildMaterials.Views
{
    public partial class LoginView : FluentWindow
    {
        public LoginView()
        {
            DataContext = new ViewModels.LoginViewModel(this);
            InitializeComponent();
        }
    }
}