using BuildMaterials.Extensions;
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
            Title = "Добавление договора купли-продажи";
        }
        public AddContractViewModel(AddContractView window, Contract contract)
        {
            _window = window;
            Contract = contract;
            Title = "Изменение договора купли-продажи";
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

        public Organization? Buyer
        {
            get => Contract.Buyer;
            set
            {                
                Contract.Buyer = value;
                if (value != null)
                {
                    _window.buyerText.Visibility = System.Windows.Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }

        public Individual? Individual
        {
            get => Contract.Individual;
            set
            {
                Contract.Individual = value;
                if (value != null) _window.buyerText.Visibility = System.Windows.Visibility.Collapsed;
                OnPropertyChanged();
            }
        }

        private async Task AddMaterial(object? obj)
        {
            try
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
            catch(Exception ex)
            {
                _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
            }
        }
        private async Task EditMaterial(object? obj)
        {
            try
            {
                ContractMaterial? Selected = _window.materialsDataGrid.SelectedItem as ContractMaterial;
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
            catch(Exception ex)
            {
                _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
            }
        }
        private async Task DeleteMaterial(object? obj)
        {
            try
            {
                ContractMaterial? Selected = _window.materialsDataGrid.SelectedValue as ContractMaterial;
                if (Selected == null) return;

                Contract.Materials.Remove(Selected);
                var arr = Contract.Materials.ToArray();
                Contract.Materials = arr.ToList();
            }
            catch (Exception ex)
            {
                _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
            }
        }
        private async Task Close(object? obj = null) => _window.DialogResult = true;
        private async Task AddContract(object? obj)
        {
            if (Contract.IsValid)
            {
                try
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
                catch (Exception ex)
                {
                    _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
                }
            }
            else
            {
                _window.ShowDialogAsync("Введена не вся требуемая информация!", Title);
            }
        }
    }
}