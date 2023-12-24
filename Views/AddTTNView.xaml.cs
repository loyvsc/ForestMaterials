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
        public AddTTNView(Models.TTN ttn)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTTNViewModel(this,ttn);
        }
    }
}