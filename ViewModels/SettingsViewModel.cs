using BuildMaterials.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Windows.ApplicationModel.Contacts;

namespace BuildMaterials.ViewModels
{
    public class SettingsViewModel
    {
        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand DBDropCommand => new RelayCommand((sender) => DropDB());

        private readonly Window _window = null!;

        public Settings Settings { get; private set; } = new Settings();

        public SettingsViewModel() { }

        public SettingsViewModel(Window window)
        {
            _window = window;
        }

        private void DropDB()
        {
            App.DbContext.Query("DROP DATABASE buildmaterials;");
            App.DbContext.CreateDatabase();

            var dc = ((MainWindowViewModel)App.Current.MainWindow.DataContext);

            switch (dc.SelectedTabAsString)
            {
                case "materialResponsibleTab":
                    {
                        dc.MaterialResponsesList = App.DbContext.MaterialResponse.ToList();
                        break;
                    }
                case "materialsTab":
                    {
                        dc.MaterialsList = App.DbContext.Materials.ToList();
                        break;
                    }
                case "employersTab":
                    {
                        dc.EmployeesList = App.DbContext.Employees.ToList();
                        break;
                    }
                case "orgTab":
                    {
                        dc.OrganizationsList = App.DbContext.Organizations.Select("SELECT * FROM sellers;");
                        break;
                    }
                case "uchetTab":
                    {
                        dc.TradesList = App.DbContext.Trades.ToList();
                        break;
                    }
                case "ttnTab":
                    {
                        dc.TTNList = App.DbContext.TTNs.ToList();
                        break;
                    }
                case "accountTab":
                    {
                        dc.AccountsList = App.DbContext.Accounts.ToList();
                        break;
                    }
                case "contractTab":
                    {
                        dc.ContractsList = App.DbContext.Contracts.ToList();
                        break;
                    }
            }
        }
    }
}