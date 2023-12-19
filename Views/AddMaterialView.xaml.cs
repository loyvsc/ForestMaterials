using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddMaterialView : Window
    {        
        public AddMaterialView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddMaterialViewModel(this);
        }

        public AddMaterialView(Models.Material material)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddMaterialViewModel(this, material);
        }
    }
}