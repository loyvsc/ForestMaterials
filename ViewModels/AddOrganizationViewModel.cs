using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;
using UNPRead;

namespace BuildMaterials.ViewModels
{
    public static partial class Extensions
    {
        public static Organization ToProvider(this UNPRead.UNP unp)
        {
            return new Organization(0, unp.FullName, unp.ShortName, unp.Adress, unp.RegistrationDate, unp.MNSNumber, unp.MNSName, unp.UNPCode);
        }
    }

    public class AddOrganizationViewModel
    {
        public Models.Organization Provider { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand(AddMaterial);

        private readonly Window _window = null!;

        public AddOrganizationViewModel()
        {
            Provider = new Models.Organization();
        }

        public AddOrganizationViewModel(Window window) : this()
        {
            _window = window;
        }

        private async void AddMaterial(object obj)
        {
            if (Provider.UNP!="")
            {
                UNPReader reader = new UNPReader();
                try
                {
                    var converted = reader.GetInfoByUNP(Provider.UNP).ToProvider();
                    Provider = converted;
                }
                catch (ArgumentException nEx)
                {
                    System.Windows.MessageBox.Show(nEx.Message, "Ошибка при получении информации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                App.DbContext.Sellers.Add(Provider);
                _window.DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Введите УНП!", "Добавление поставщика", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}