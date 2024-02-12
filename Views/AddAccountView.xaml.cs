using System.Windows;
using System.Windows.Controls;

namespace BuildMaterials.Views
{
    public partial class AddAccountView : FluentWindow
    {
        public AddAccountView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddAccountViewModel(this);
        }

        private void DatePicker_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var datepicker = sender as DatePicker;
            dateText.Visibility = datepicker.Text.Length==0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}