using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddEmployeeViewModel
    {
        public Models.Employee Employee { get; set; }

        public ICommand CancelCommand => new RelayCommand((sender) => _window.Close());
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());        

        private readonly Window _window = null!;
        public string[] EmployeeAccessLevel => App.DbContext.AccessLevel;
        public int SelectedAccessLevel { get; set; }

        public AddEmployeeViewModel()
        {
            Employee = new Models.Employee();
        }

        public AddEmployeeViewModel(Window window) : this()
        {
            _window = window;
        }

        private void AddMaterial()
        {
            Employee.AccessLevel = SelectedAccessLevel;
            if (Employee.IsValid)
            {
                App.DbContext.Employees.Add(Employee);
                _window.DialogResult = true;
                return;
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый сотрудник", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}