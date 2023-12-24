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
        public readonly Settings Settings;

        public List<Organization> CustomersList
        {
            get
            {
                List<Organization> customers = new List<Organization>(128);
                using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
                {
                    _connection.Open();
                    using (MySqlCommand _command = new MySqlCommand
                        ("SELECT CompanyName, Adress FROM customers union SELECT CompanyName, Adress FROM providers;", _connection))
                    {
                        using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                            while (reader.Read())
                            {
                                customers.Add(new Organization() { CompanyName = reader.GetString(0), Adress = reader.GetString(1) });
                            }
                    }
                    _connection.Close();
                }
                customers.Add(new Organization() { CompanyName = Settings.CompanyName, Adress = Settings.CompanyAdress });
                return customers;
            }
        }

        public List<Employee> Employees => App.DbContext.Employees.ToList();

        public int SelectedShipperIndex { get; set; } = -1;
        public int SelectedConsigneeIndex { get; set; } = -1;
        public List<Material> Materials => App.DbContext.Materials.ToList();

        public AddAccountViewModel()
        {
            Settings = new Settings();
        }

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