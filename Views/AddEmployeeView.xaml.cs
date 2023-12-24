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
        public AddEmployeeView(Models.Employee employee)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddEmployeeViewModel(this, employee);
        }
    }
}