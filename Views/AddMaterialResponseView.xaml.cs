using BuildMaterials.ViewModels;
using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddMaterialResponseView : Window
    {
        public AddMaterialResponseView()
        {
            InitializeComponent();
            this.DataContext = new AddMaterialResponseViewModel(this);
        }
    }
}
