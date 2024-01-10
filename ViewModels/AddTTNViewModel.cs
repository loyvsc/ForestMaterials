using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public static class Extensions
    {
        public static MySqlDataReader ExecuteMySqlReaderAsync(this MySqlCommand command) => command.ExecuteReader();
    }

    public class AddTTNViewModel : NotifyPropertyChangedBase
    {
        private readonly Window _window = null!;
        public TTN TTN
        {
            get=> ttn;
            set
            {
                ttn = value;
                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand(AddMaterial);

        public List<Contract> Contracts { get; } = App.DbContext.Contracts.ToList();
        public List<Automobile> Automobiles { get; } = App.DbContext.Automobiles.ToList();
        public List<Employee> Employees { get; } = App.DbContext.Employees.ToList();
        public List<Organization>? CustomersList { get; } = App.DbContext.Organizations.ToList();

        private TTN ttn;

        private void Close(object? obj = null) => _window.DialogResult = true;

        public AddTTNViewModel()
        {
            TTN = new TTN();
        }

        public AddTTNViewModel(Window window) : this()
        {
            _window = window;
        }
        public AddTTNViewModel(Window window, Models.TTN ttn) : this(window)
        {
            TTN = ttn;
        }

        private void AddMaterial(object? obj)
        {
            if (TTN.IsValid)
            {
                try
                {

                    if (TTN.ID != 0)
                    {
                        App.DbContext.TTNs.Update(TTN);
                    }
                    else
                    {
                        App.DbContext.TTNs.Add(TTN);
                    }
                    Close();
                }
                catch
                {
                    System.Windows.MessageBox.Show("Произошла ошибка. Попробуйте позже", "Новый ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый ТТН", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}