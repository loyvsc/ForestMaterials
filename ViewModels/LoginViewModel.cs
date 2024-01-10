using BuildMaterials.BD;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class LoginViewModel : NotifyPropertyChangedBase
    {
        public List<Employee> Employees => GetEmployees();
        public ICommand AutorizeCommand => new RelayCommand((sender) => Autorize());
        public int SelectedTypeIndex { get; set; }
        public string? EnteredPassword
        {
            get => _enteredPassword;
            set
            {
                _enteredPassword = value;
                OnPropertyChanged();
            }
        }

        private List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>(32);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                using (MySqlCommand _command = new MySqlCommand("SELECT concat(surname,' ', name,' ',pathnetic), Password, canadd, canedit, candel, isadmin FROM Employees;", _connection))
                {
                    _connection.OpenAsync().Wait();
                    using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                        while (reader.Read())
                        {
                            employees.Add(new Employee(reader.GetString(0), reader.GetString(1), (reader.GetInt32(2) == 1 ? true : false), (reader.GetInt32(3) == 1 ? true : false), (reader.GetInt32(4) == 1 ? true : false), (reader.GetInt32(5) == 1 ? true : false)));
                        }
                    _connection.CloseAsync().Wait();
                }
            }
            return employees;
        }

        public LoginViewModel()
        {
            EnteredPassword = "";
        }

        public LoginViewModel(Window parentWindow) : this()
        {
            _window = parentWindow;
        }

        private readonly Window? _window;
        private string? _enteredPassword;

        private bool IsSelectedTypeValid => SelectedTypeIndex.Equals(-1);

        public void Autorize()
        {
            try
            {
                if (IsSelectedTypeValid)
                {
                    throw new AutorizeException("Выберите пользователя!");
                }

                Employee findedEmployee = Employees[SelectedTypeIndex];

                if (EnteredPassword == findedEmployee.Password)
                {
                    MainWindow mainWindow = new MainWindow(findedEmployee);
                    mainWindow.Show();
                    System.Windows.Application.Current.MainWindow = mainWindow;
                    _window?.Close();
                }
                else
                {
                    throw new AutorizeException("Неверный пароль!");
                }
            }
            catch (AutorizeException aEx)
            {
                System.Windows.MessageBox.Show(aEx.Message, "Авторизация", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> execute;
        private readonly Func<object?, bool>? canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }

    public class AutorizeException : Exception
    {
        public readonly new string Message;

        public AutorizeException(string message)
        {
            Message = message;
        }
    }
}