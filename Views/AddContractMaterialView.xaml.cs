using BuildMaterials.Models;
using BuildMaterials.ViewModels;
using System.Windows;

namespace BuildMaterials.Views
{
    public partial class AddContractMaterialView : Window
    {
        public readonly AddContractMaterialViewModel viewModel;
        private AddContractMaterialView()
        {
            InitializeComponent();
        }
        public AddContractMaterialView(int contractId) : this()
        {
            viewModel = new AddContractMaterialViewModel(this, contractId);
            DataContext = viewModel;
        }
        public AddContractMaterialView(ContractMaterial contractMaterial) : this()
        {
            viewModel = new AddContractMaterialViewModel(this, contractMaterial);
            DataContext = viewModel;
        }
    }
}
