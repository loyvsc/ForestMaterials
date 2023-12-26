using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddEmployeeViewModel
    {
        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand(AddMaterial);

        public Models.Employee Employee { get; set; }

        private readonly Window _window = null!;
        public AddEmployeeViewModel()
        {
            Employee = new Models.Employee();
        }

        public AddEmployeeViewModel(Window window) : this()
        {
            _window = window;
        }

        public AddEmployeeViewModel(Window window, Models.Employee employee)
        {
            _window = window;
            Employee = employee;
        }

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void AddMaterial(object? obj)
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
                    System.Windows.MessageBox.Show("При сохранении изменений произошла ошибка...\nСообщение: " + ex.Message, "Новый сотрудник", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (Employee.IsValid)
                {
                    App.DbContext.Employees.Add(Employee);
                    Close();
                    return;
                }
                System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый сотрудник", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}