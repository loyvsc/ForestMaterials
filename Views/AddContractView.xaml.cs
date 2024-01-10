using BuildMaterials.Models;
using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddContractView : Window
    {
        public AddContractView()
        {
            InitializeComponent();
            DataContext = new ViewModels.AddContractViewModel(this);
        }
        public AddContractView(Contract contract)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddContractViewModel(this, contract);
        }
    }
}