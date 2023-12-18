using BuildMaterials.Models;
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
        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;
        public readonly Settings Settings;
        public Contract contr;

        public List<Material> Materials => App.DbContext.Materials.ToList();
        public List<Organization> CustomersList => App.DbContext.Sellers.ToList(true);
        public List<Organization> ProvidersList => App.DbContext.Sellers.ToList();

        public int SelectedShipperIndex;
        public int SelectedConsigneeIndex;

        public AddContractViewModel()
        {
            Settings = new Settings();
            Contract = new Contract();
        }

        public AddContractViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            if (Contract.IsValid)
            {
                App.DbContext.Contracts.Add(Contract);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый счет-фактура", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}