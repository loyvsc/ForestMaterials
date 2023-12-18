using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public static partial class Extensions
    {
        public static MySqlDataReader ExecuteMySqlReaderAsync(this MySqlCommand command)
        {
            return (MySqlDataReader)command.ExecuteReaderAsync().Result;
        }
    }

    public class AddTTNViewModel
    {
        public Models.TTN TTN { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;
        public List<Material> Materials => App.DbContext.Materials.ToList();

        public readonly Settings Settings = new Settings();
        public List<Organization>? CustomersList { get; set; }

        public AddTTNViewModel()
        {
            TTN = new TTN();
        }

        public AddTTNViewModel(Window window) : this()
        {
            _window = window;
            CustomersList = App.DbContext.Sellers.ToList();
        }

        ~AddTTNViewModel()
        {
            CustomersList = null;
        }

        private void AddMaterial()
        {
            if (TTN.IsValid)
            {
                App.DbContext.TTNs.Add(TTN);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}