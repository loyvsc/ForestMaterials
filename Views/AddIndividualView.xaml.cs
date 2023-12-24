using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddIndividualView : Window
    {
        public AddIndividualView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddIndividualViewModel(this);
        }
        public AddIndividualView(Models.Individual employee)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddIndividualViewModel(this, employee);
        }
    }
}