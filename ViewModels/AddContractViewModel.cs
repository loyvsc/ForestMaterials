﻿using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContractViewModel : ViewModelBase
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
        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddContract);

        public ICommand AddMaterialCommand => new AsyncRelayCommand(AddMaterial);
        public ICommand EditMaterialCommand => new AsyncRelayCommand(EditMaterial);
        public ICommand DeleteMaterialCommand => new AsyncRelayCommand(DeleteMaterial);
        #endregion

        #region Private vars
        private readonly AddContractView _window;
        private Contract contr;
        #endregion

        #region Lists
        public List<Material> Materials => App.DbContext.Materials.ToList();
        public List<Organization> OrganizationsList => App.DbContext.Organizations.ToList();
        public List<Employee> Employees => App.DbContext.Employees.ToList();
        public List<Individual> Individuals => App.DbContext.Individuals.ToList();
        public List<string> LogisticsTypes => new List<string>(3)
        {
            "Франко-верхний лесосклад", "Франко-промежуточный лесосклад",
            "Франко-склад организации-изготовителя "
        };
        #endregion

        #region Constructors
        public AddContractViewModel(AddContractView window)
        {
            _window = window;
            Contract = new Contract();
            Title = "Добавление счета-фактуры";
        }
        public AddContractViewModel(AddContractView window, Contract contract)
        {
            _window = window;
            Contract = contract;
            Title = "Изменение счета-фактуры";
        }
        #endregion

        public DateTime? Date
        {
            get => Contract.Date;
            set
            {
                if (value != null)
                {
                    Contract.Date = value;
                    _window.dateText.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public string? LogisticsType
        {
            get => Contract.LogisiticsType;
            set
            {
                if (value != null)
                {
                    Contract.LogisiticsType = value;
                    _window.logText.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public int? Seller
        {
            get => Contract.SellerID;
            set
            {
                if (value != null)
                {
                    Contract.SellerID = (int) value;
                    _window.sellerText.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public int? Buyer
        {
            get => Contract.BuyerID;
            set
            {
                if (value != null)
                {
                    Contract.BuyerID = (int) value;
                    _window.buyerText.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public Individual? Individual
        {
            get => Contract.Individual;
            set
            {
                if (value != null)
                {
                    Contract.Individual = value;
                    _window.buyerText.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private async Task AddMaterial(object? obj)
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
        private async Task EditMaterial(object? obj)
        {
            ContractMaterial? Selected = _window.materialsDataGrid.SelectedValue as ContractMaterial;
            if (Selected == null) return;
            AddContractMaterialView view = new AddContractMaterialView(Selected);
            if (view.ShowDialog() == true)
            {
                var i = Contract.Materials.FindIndex((x) => x == Selected);
                Contract.Materials[i] = view.viewModel.ContractMaterial;
                var arr = Contract.Materials.ToArray();
                Contract.Materials = arr.ToList();
            }
        }
        private async Task DeleteMaterial(object? obj)
        {
            ContractMaterial? Selected = _window.materialsDataGrid.SelectedValue as ContractMaterial;
            if (Selected == null) return;

            Contract.Materials.Remove(Selected);
            var arr = Contract.Materials.ToArray();
            Contract.Materials = arr.ToList();
        }
        private async Task Close(object? obj = null) => _window.DialogResult = true;
        private async Task AddContract(object? obj)
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
                _window.ShowDialogAsync("Введена не вся требуемая информация!", Title);
            }
        }
    }
}