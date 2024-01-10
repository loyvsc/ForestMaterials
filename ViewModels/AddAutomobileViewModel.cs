using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddAutomobileViewModel : NotifyPropertyChangedBase
    {
        public Automobile Automobile { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand AddCommand { get; }

        private readonly AddAutomobileView _window;

        public AddAutomobileViewModel()
        {
            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Close);
        }

        public AddAutomobileViewModel(AddAutomobileView view) : this()
        {
            _window = view;
            _window.addComm.Content = "Добавить";
            _window.Title = "Добавление автомобиля";
            Automobile = new Automobile();
        }

        public AddAutomobileViewModel(AddAutomobileView window, Automobile automobile) : this()
        {
            Automobile = automobile;
            _window = window;
            _window.Title = "Редактирование автомобиля";
            _window.addComm.Content = "Сохранить";
        }

        private void Close(object? obj) => _window.DialogResult = true;
        private void Add(object? obj)
        {
            if (Automobile.IsValid)
            {
                try
                {
                    if (Automobile.ID != 0)
                    {
                        App.DbContext.Automobiles.Update(Automobile);
                    }
                    else
                    {
                        App.DbContext.Automobiles.Add(Automobile);
                    }
                    Close(null);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Попробуйте позже.", _window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Введите всю требуемую информацию!", _window.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
