using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddIndividualViewModel
    {
        public Individual Individual { get; set; }

        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand(AddMaterial);

        private readonly Window _window = null!;

        public AddIndividualViewModel()
        {
            Individual = new Individual();
        }

        public AddIndividualViewModel(Window window) : this()
        {
            _window = window;
        }

        public AddIndividualViewModel(Window window, Individual employee)
        {
            _window = window;
            Individual = employee;
        }

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void AddMaterial(object? obj)
        {
            if (Individual.ID != 0)
            {
                try
                {
                    App.DbContext.Individuals.Update(Individual);
                    Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("При сохранении изменений произошла ошибка...\nСообщение: " + ex.Message, "Новое физ. лицо", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (Individual.IsValid)
                {
                    App.DbContext.Individuals.Add(Individual);
                    Close();
                    return;
                }
                System.Windows.MessageBox.Show("Не вся информация была введена!", "Новое физ. лицо", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}