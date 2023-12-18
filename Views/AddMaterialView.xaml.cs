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
    }
}