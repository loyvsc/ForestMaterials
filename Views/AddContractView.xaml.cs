using BuildMaterials.Models;
using BuildMaterials.ViewModels;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BuildMaterials.Views
{
    public partial class AddContractView : FluentWindow
    {
        private readonly AddContractViewModel viewModel;
        public AddContractView()
        {
            InitializeComponent();
            viewModel = new AddContractViewModel(this);
            DataContext = viewModel;
        }
        public AddContractView(Contract contract)
        {
            InitializeComponent();
            viewModel = new AddContractViewModel(this, contract);
            DataContext = viewModel;
        }

        private void isBuyerIdv_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Individual = null;
            viewModel.Buyer = null;
            buyerText.Visibility = System.Windows.Visibility.Visible;
        }

        private void DatePicker_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textbox = sender as DatePicker;
            dateText.Visibility = textbox.Text.Length != 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }
    }
}