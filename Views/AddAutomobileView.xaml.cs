using BuildMaterials.Models;
using BuildMaterials.ViewModels;
using System.Windows;

namespace BuildMaterials.Views
{
    /// <summary>
    /// Логика взаимодействия для AddAutomobileView.xaml
    /// </summary>
    public partial class AddAutomobileView : Window
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
