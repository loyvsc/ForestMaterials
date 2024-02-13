using BuildMaterials.Extensions;
using BuildMaterials.Helpers;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddIndividualViewModel : ViewModelBase
    {
        public PhoneNumberInputHelper PhoneNumberInput { get; set; } = new PhoneNumberInputHelper();
        public Individual Individual { get; set; }

        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);

        public DateTime? IssueDate
        {
            get => Individual.Passport.IssueDate;
            set
            {
                if (value != null)
                {
                    Individual.Passport.IssueDate = value;
                    _window.dateText.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private readonly AddIndividualView _window;

        public AddIndividualViewModel(AddIndividualView window)
        {
            Title = "Добавление физ. лица";
            Individual = new Individual();
            _window = window;
        }

        public AddIndividualViewModel(AddIndividualView window, Individual employee)
        {
            _window = window;
            Title = "Изменение физ. лица";
            PhoneNumberInput.Phone = employee.PhoneNumber;
            Individual = employee;
        }

        private async Task Close(object? obj = null) => _window.DialogResult = false;

        private async Task AddMaterial(object? obj)
        {
            Individual.PhoneNumber = PhoneNumberInput.Phone;

            if (Individual.IsValid)
            {
                try
                {
                    if (Individual.ID != 0)
                    {
                        App.DbContext.Individuals.Update(Individual);
                    }
                    else
                    {
                        App.DbContext.Individuals.Add(Individual);
                    }
                    _window.DialogResult = true;
                }
                catch (Exception ex)
                {
                    _window.ShowDialogAsync("Произошла ошибка...\nСообщение: " + ex.Message, Title);
                }
            }
            else
            {
                _window.ShowDialogAsync("Не вся информация была введена!", Title);
            }            
        }
    }
}