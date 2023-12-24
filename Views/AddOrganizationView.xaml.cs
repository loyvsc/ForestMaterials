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
        public AddOrganizationView(Models.Organization organization)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddOrganizationViewModel(this, organization);
        }
    }
}