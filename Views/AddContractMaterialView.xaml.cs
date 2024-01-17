using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.Views
{
    public partial class AddContractMaterialView : FluentWindow
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
