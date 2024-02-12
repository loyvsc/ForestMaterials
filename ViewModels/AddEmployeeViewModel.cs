using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddEmployeeViewModel : ViewModelBase
    {
        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);

        public Models.Employee Employee { get; set; }
        public DateTime? IssueDate
        {
            get => Employee.Passport.IssueDate;
            set
            {
                if (value != null)
                {
                    Employee.Passport.IssueDate = value;
                    _window.dateText.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private readonly AddEmployeeView _window = null!;

        public AddEmployeeViewModel(AddEmployeeView window)
        {
            Title = "Добавление сотрудника";
            _window = window;
            Employee = new Models.Employee();
        }

        public AddEmployeeViewModel(AddEmployeeView window, Employee employee)
        {
            Title = "Изменение сотрудника";
            _window = window;
            Employee = employee;
        }

        private async Task Close(object? obj = null) => _window.DialogResult = true;

        private async Task AddMaterial(object? obj)
        {
            if (Employee.IsValid)
            {
                if (Employee.ID != 0)
                {
                    try
                    {
                        App.DbContext.Employees.Update(Employee);
                        if ((App.Current.MainWindow.DataContext as MainWindowViewModel)?.CurrentEmployee?.ID == Employee.ID)
                        {
                            (App.Current.MainWindow.DataContext as MainWindowViewModel).CurrentEmployee = Employee;
                        }
                        Close();
                    }
                    catch (Exception ex)
                    {
                        _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
                    }
                }
                else
                {
                    try
                    {
                        App.DbContext.Employees.Add(Employee);
                        Close();
                    }
                    catch (Exception ex)
                    {
                        _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
                    }
                }
            }
            else
            {
                _window.ShowDialogAsync("Введена не вся требуемая информация!", Title);
            }            
        }
    }
}