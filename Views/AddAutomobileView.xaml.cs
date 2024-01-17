using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.Views
{
    /// <summary>
    /// Логика взаимодействия для AddAutomobileView.xaml
    /// </summary>
    public partial class AddAutomobileView : FluentWindow
    {
        public AddAutomobileView()
        {
            InitializeComponent();
            DataContext = new AddAutomobileViewModel(this);
        }
        public AddAutomobileView(Automobile automobile)
        {
            InitializeComponent();
            DataContext = new AddAutomobileViewModel(this, automobile);
        }
    }
}
