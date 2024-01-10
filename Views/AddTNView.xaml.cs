using BuildMaterials.Models;
using BuildMaterials.ViewModels;
using System.Windows;

namespace BuildMaterials.Views
{
    /// <summary>
    /// Логика взаимодействия для AddTNView.xaml
    /// </summary>
    public partial class AddTNView : Window
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
