using System.Windows;

namespace BuildMaterials.Views
{
    public partial class SettingsView : Window
    {        
        public SettingsView()
        {
            InitializeComponent();
            DataContext = new ViewModels.SettingsViewModel(this);
        }
    }
}