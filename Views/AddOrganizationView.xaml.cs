using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddOrganizationView : Window
    {        
        public AddOrganizationView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddOrganizationViewModel(this);
        }
    }
}