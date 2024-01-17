using BuildMaterials.Models;
using BuildMaterials.ViewModels;

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
            viewModel.Buyer = 0;
            buyerText.Visibility = System.Windows.Visibility.Visible;
        }
    }
}