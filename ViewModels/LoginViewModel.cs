using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public List<Employee> Employees { get; }
        public ICommand AutorizeCommand => new AsyncRelayCommand(Autorize);
        public string? EnteredPassword
        {
            get => _enteredPassword;
            set
            {
                if (_window.IsLoaded)
                {
                    _window.passwordBox.BorderBrush = System.Windows.Media.Brushes.Transparent;
                    _window.passwordBox.BorderThickness = new Thickness(0);
                }
                _enteredPassword = value;
                OnPropertyChanged();
            }
        }
        public Employee? SelectedEmploee
        {
            get => selEmpl;
            set
            {
                if (value != null)
                {
                    if (_window.IsLoaded)
                    {
                        _window.emplCombobox.BorderBrush = System.Windows.Media.Brushes.Transparent;
                        _window.emplCombobox.BorderThickness = new Thickness(0,0,0,0);
                    }
                    _window.emplTextBlock.Visibility = Visibility.Collapsed;
                    selEmpl = value;
                    OnPropertyChanged();
                }
            }
        }

        public LoginViewModel(LoginView parentWindow)
        {
            _window = parentWindow;
            EnteredPassword = "";
            Title = "Авторизация";
            Employees = App.DbContext.Employees.ToList();
        }

        private readonly LoginView _window;
        private string? _enteredPassword;
        private Employee? selEmpl;

        public async Task Autorize(object? obj)
        {
            try
            {
                if (SelectedEmploee == null)
                {
                    _window.emplCombobox.BorderBrush = System.Windows.Media.Brushes.DarkRed;
                    _window.emplCombobox.BorderThickness = new Thickness(0,0,0,2);
                    throw new AutorizeException("Выберите пользователя!");

                }

                if (EnteredPassword == SelectedEmploee.Password)
                {
                    MainWindow mainWindow = new MainWindow(SelectedEmploee);
                    mainWindow.Show();
                    System.Windows.Application.Current.MainWindow = mainWindow;
                    _window?.Close();
                }
                else
                {
                    _window.passwordBox.BorderBrush = System.Windows.Media.Brushes.DarkRed;
                    _window.passwordBox.BorderThickness = new Thickness(0, 0, 0, 2);
                    throw new AutorizeException("Неверный пароль!");
                }
            }
            catch (AutorizeException aEx)
            {
                _window.ShowDialogAsync(aEx.Message, Title);
            }
        }
    }

    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> execute;
        private readonly Func<object, bool> canExecute;
        private long isExecuting;
        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute ?? (o => true);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
        public bool CanExecute(object parameter)
        {
            if (Interlocked.Read(ref isExecuting) != 0)
                return false;
            return canExecute(parameter);
        }
        public async void Execute(object parameter)
        {
            Interlocked.Exchange(ref isExecuting, 1);
            RaiseCanExecuteChanged();
            try
            {
                await execute(parameter);
            }
            finally
            {
                Interlocked.Exchange(ref isExecuting, 0);
                RaiseCanExecuteChanged();
            }
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