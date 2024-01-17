using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddAutomobileViewModel : ViewModelBase
    {
        public Automobile Automobile { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand AddCommand { get; }

        private readonly AddAutomobileView _window;

        public AddAutomobileViewModel()
        {
            AddCommand = new AsyncRelayCommand(Add);
            CancelCommand = new AsyncRelayCommand(Close);
        }

        public AddAutomobileViewModel(AddAutomobileView view) : this()
        {
            _window = view;
            _window.addComm.Content = "Добавить";
            Title = "Добавление автомобиля";
            Automobile = new Automobile();
        }

        public AddAutomobileViewModel(AddAutomobileView window, Automobile automobile) : this()
        {
            Automobile = automobile;
            _window = window;
            Title = "Изменение автомобиля";
            _window.addComm.Content = "Сохранить";
        }

        private async Task Close(object? obj) => _window.DialogResult = true;
        private async Task Add(object? obj)
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
