using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContractMaterialViewModel : NotifyPropertyChangedBase
    {
        public ICommand AddCommand => new RelayCommand(Add);
        public ICommand CloseCommand => new RelayCommand(Close);

        #region Public proprs
        public int MaterialID
        {
            get => matid;
            set
            {
                matid = value;
                OnPropertyChanged();
            }
        }
        private int matid;
        public List<Material> Materials => App.DbContext.Materials.ToList();
        public ContractMaterial ContractMaterial
        {
            get => contract;
            set
            {
                contract = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Private vars
        private ContractMaterial contract;
        private readonly AddContractMaterialView _view;
        #endregion

        #region Constructors
        private AddContractMaterialViewModel(AddContractMaterialView view)
        {
            _view = view;
        }

        public AddContractMaterialViewModel()
        {
            ContractMaterial = new ContractMaterial();
            _view = null!;
        }
        public AddContractMaterialViewModel(AddContractMaterialView view, int contractId) : this(view)
        {
            ContractMaterial = new ContractMaterial()
            {
                ContactID = contractId
            };
        }
        public AddContractMaterialViewModel(AddContractMaterialView view, ContractMaterial contractMaterial) : this()
        {
            ContractMaterial = contractMaterial;
            MaterialID = contractMaterial.Material.ID;
            _view = view;
        }
        #endregion

        #region Private methods
        private void Add(object? obj)
        {
            if (ContractMaterial.Material!=null) ContractMaterial.Material.ID = 0;
            if (ContractMaterial.IsValid)
            {
                ContractMaterial.Material = Materials.Find((x) => x.ID == MaterialID);
                Close(null);
            }
            else
            {
                System.Windows.MessageBox.Show("Введите всю требуемую информацию!", "Добавление товара", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error); 
            }
        }
        private void Close(object? obj)
        {
            _view.DialogResult = true;
        }
        #endregion
    }
}
