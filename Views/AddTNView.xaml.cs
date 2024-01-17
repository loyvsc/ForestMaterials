using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.Views
{
    /// <summary>
    /// Логика взаимодействия для AddTNView.xaml
    /// </summary>
    public partial class AddTNView : FluentWindow
    {
        public AddTNView()
        {
            InitializeComponent();
            DataContext = new AddTNViewModel(this);
        }
        public AddTNView(TN tn)
        {
            InitializeComponent();
            DataContext = new AddTNViewModel(this, tn);
        }
    }
}
