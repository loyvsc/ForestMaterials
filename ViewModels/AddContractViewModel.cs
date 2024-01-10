using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContractViewModel : NotifyPropertyChangedBase
    {
        public Contract Contract
        {
            get => contr;
            set
            {
                contr = value;
                OnPropertyChanged(nameof(Contract));
            }
        }

        #region Commands
        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand(AddContract);

        public ICommand AddMaterialCommand => new RelayCommand(AddMaterial);
        public ICommand EditMaterialCommand => new RelayCommand(EditMaterial);
        public ICommand DeleteMaterialCommand => new RelayCommand(DeleteMaterial);
        #endregion

        #region Private vars
        private readonly AddContractView _window;
        private Contract contr;
        #endregion

        public List<Material> Materials => App.DbContext.Materials.ToList();
        public List<Organization> OrganizationsList => App.DbContext.Organizations.ToList();
        public List<Employee> Employees => App.DbContext.Employees.ToList();
        public List<Individual> Individuals => App.DbContext.Individuals.ToList();
        public List<string> LogisticsTypes => new List<string>(4)
        {
            "Франко-верхний лесосклад", "Франко-промежуточный лесосклад",
            "Франко-склад организации-изготовителя "
        };

        #region Constructors
        public AddContractViewModel()
        {
            Contract = new Contract();
        }

        public AddContractViewModel(AddContractView window) : this()
        {
            _window = window;
        }
        public AddContractViewModel(AddContractView window, Contract contract)
        {
            _window = window;
            Contract = contract;
        }
        #endregion

        private void AddMaterial(object? obj)
        {
            AddContractMaterialView view = new AddContractMaterialView(Contract.ID);
            if (view.ShowDialog() == true)
            {
                Contract.Materials.Add(view.viewModel.ContractMaterial);
                var arr = Contract.Materials.ToArray();
                Contract.Materials = arr.ToList();
                OnPropertyChanged(nameof(Contract.Materials));
            }
        }
        private void EditMaterial(object? obj)
        {
            ContractMaterial? selval = _window.materialsDataGrid.SelectedValue as ContractMaterial;
            if (selval == null) return;
            AddContractMaterialView view = new AddContractMaterialView(selval);
            if (view.ShowDialog() == true)
            {
                var i = Contract.Materials.FindIndex((x) => x == selval);
                Contract.Materials[i] = view.viewModel.ContractMaterial;
                var arr = Contract.Materials.ToArray();
                Contract.Materials = arr.ToList();
            }
        }
        private void DeleteMaterial(object? obj)
        {
            ContractMaterial? selval = _window.materialsDataGrid.SelectedValue as ContractMaterial;
            if (selval == null) return;

            Contract.Materials.Remove(selval);
            var arr = Contract.Materials.ToArray();
            Contract.Materials = [.. arr];
        }

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void AddContract(object? obj)
        {
            if (Contract.IsValid)
            {
                if (Contract.ID != 0)
                {
                    App.DbContext.Contracts.Update(Contract);
                }
                else
                {
                    App.DbContext.Contracts.Add(Contract);
                }
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый счет-фактура", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}