using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddTTNView : Window
    {
        public AddTTNView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTTNViewModel(this);
        }
    }
}