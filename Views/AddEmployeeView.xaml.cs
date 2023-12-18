using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddEmployeeView : Window
    {        
        public AddEmployeeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddEmployeeViewModel(this);
        }
    }
}