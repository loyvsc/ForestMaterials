using System.Text.RegularExpressions;

namespace BuildMaterials.Views
{
    public partial class AddIndividualView : FluentWindow
    {
        public AddIndividualView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddIndividualViewModel(this);
        }
        public AddIndividualView(Models.Individual employee)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddIndividualViewModel(this, employee);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}