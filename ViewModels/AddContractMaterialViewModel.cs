using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContractMaterialViewModel : ViewModelBase
    {
        public ICommand AddCommand => new AsyncRelayCommand(Add);
        public ICommand CloseCommand => new AsyncRelayCommand(Close);

        private Visibility _textVis;

        #region Public proprs
        public Visibility TextVisibility
        {
            get => _textVis;
            set
            {
                _textVis = value;
                OnPropertyChanged();
            }
        }
        public int? MaterialID
        {
            get => matid;
            set
            {
                if (value != null)
                {
                    TextVisibility = System.Windows.Visibility.Collapsed;
                    matid = value;
                    OnPropertyChanged();
                }
            }
        }
        private int? matid;
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
            Title = "Добавление лесопродукта";
            ContractMaterial = new ContractMaterial()
            {
                ContactID = contractId
            };
        }
        public AddContractMaterialViewModel(AddContractMaterialView view, ContractMaterial contractMaterial) : this()
        {
            Title = "Изменение лесопродукта";
            ContractMaterial = contractMaterial;
            MaterialID = contractMaterial.Material.ID;
            _view = view;
        }
        #endregion

        #region Private methods
        private async Task Add(object? obj)
        {
            if (ContractMaterial.Material!=null) ContractMaterial.Material.ID = 0;
            if (ContractMaterial.IsValid)
            {
                ContractMaterial.Material = Materials.Find((x) => x.ID == MaterialID);
                Close(null);
            }
            else
            {
                _view.ShowDialogAsync("Не вся информация была введена!", Title);
            }
        }
        private async Task Close(object? obj)
        {
            _view.DialogResult = true;
        }
        #endregion
    }
}
