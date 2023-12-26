using BuildMaterials.BD;
using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddAccountViewModel
    {
        public Account Account { get; set; } = new Account();
        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;

        public List<Organization> CustomersList => App.DbContext.Organizations.ToList();

        public List<Employee> Employees => App.DbContext.Employees.ToList();

        public int SelectedShipperIndex { get; set; } = -1;
        public int SelectedConsigneeIndex { get; set; } = -1;
        public List<Material> Materials => App.DbContext.Materials.ToList();

        public AddAccountViewModel() { }

        public AddAccountViewModel(Window window) : this()
        {
            _window = window;
        }

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void AddMaterial()
        {
            Account.ShipperAdress = CustomersList[SelectedShipperIndex].Adress;
            Account.ConsigneeAdress = CustomersList[SelectedConsigneeIndex].Adress;
            if (Account.IsValid)
            {
                App.DbContext.Accounts.Add(Account);
                Close();
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый счет-фактура", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}