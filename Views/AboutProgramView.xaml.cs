using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AboutProgramView : Window
    {
        public AboutProgramView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AboutProgrammViewModel(this);
        }
    }
}