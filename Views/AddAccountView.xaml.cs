using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddAccountView : FluentWindow
    {
        public AddAccountView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddAccountViewModel(this);
        }
    }
}