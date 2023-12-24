using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public static class Extensions
    {
        public static MySqlDataReader ExecuteMySqlReaderAsync(this MySqlCommand command) => command.ExecuteReader();
    }

    public class AddTTNViewModel
    {
        public Models.TTN TTN { get; set; }

        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand(AddMaterial);

        private readonly Window _window = null!;
        public List<Material> Materials => App.DbContext.Materials.ToList();

        public readonly Settings Settings = new Settings();
        public List<Organization>? CustomersList { get; set; }

        private void Close(object? obj = null) => _window.DialogResult = true;

        public AddTTNViewModel()
        {
            TTN = new TTN();
        }

        public AddTTNViewModel(Window window) : this()
        {
            _window = window;
            CustomersList = App.DbContext.Organizations.ToList();
        }
        public AddTTNViewModel(Window window, Models.TTN ttn) : this(window)
        {
            TTN = ttn;
        }

        ~AddTTNViewModel()
        {
            CustomersList = null;
        }

        private void AddMaterial(object? obj)
        {
            if (TTN.ID != 0)
            {
                try
                {
                    App.DbContext.TTNs.Update(TTN);
                }
                catch
                {
                    System.Windows.MessageBox.Show("При сохранении изменений произошла ошибка...", "Новый ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (TTN.IsValid)
                {
                    App.DbContext.TTNs.Add(TTN);
                    Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}