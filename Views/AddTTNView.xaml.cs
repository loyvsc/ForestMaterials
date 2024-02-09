namespace BuildMaterials.Views
{
    public partial class AddTTNView : FluentWindow
    {
        public AddTTNView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTTNViewModel(this);
        }
        public AddTTNView(Models.TTN ttn)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddTTNViewModel(this, ttn);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}