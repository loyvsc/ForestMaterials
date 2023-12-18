using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddAccountView : Window
    {
        public AddAccountView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddAccountViewModel(this);
        }
    }
}