using System.Text.RegularExpressions;
using System.Windows.Controls;

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

        private void DatePicker_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textbox = sender as DatePicker;

            dateText.Visibility = textbox!.Text.Length == 0 ?
                System.Windows.Visibility.Visible :
                System.Windows.Visibility.Collapsed;
        }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex ContainsLettersRegex();

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = ContainsLettersRegex();
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}