using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BuildMaterials.Views
{
    public partial class AddEmployeeView : FluentWindow
    {
        public AddEmployeeView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddEmployeeViewModel(this);
        }
        public AddEmployeeView(Models.Employee employee)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddEmployeeViewModel(this, employee);
        }

        private void CalendarDatePicker_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textbox = sender as DatePicker;

            dateText.Visibility = textbox.Text.Length == 0 ? 
                System.Windows.Visibility.Visible :
                System.Windows.Visibility.Collapsed;
        }
    }
}