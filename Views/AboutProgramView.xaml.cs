namespace BuildMaterials.Views
{
    public partial class AboutProgramView : FluentWindow
    {
        public AboutProgramView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AboutProgrammViewModel(this);
        }
    }
}