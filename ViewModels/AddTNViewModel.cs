using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddTNViewModel : NotifyPropertyChangedBase
    {
        #region Public props
        public TN? TN { get; set; }

        public List<Contract> ContractsList => App.DbContext.Contracts.ToList();
        public List<Employee> EmployeesList => App.DbContext.Employees.ToList();

        public ICommand AddCommand => new RelayCommand(Add);
        public ICommand CloseCommand => new RelayCommand(Close);
        #endregion

        #region Private vars
        private readonly AddTNView _window;
        #endregion

        #region Constructors
        public AddTNViewModel()
        {
            TN = new TN();
        }
        public AddTNViewModel(AddTNView view) : this()
        {
            _window = view;
            _window.Title = "Добавление ТН";
            _window.addBut.Content = "Добавить";
            _window.clBut.Content = "Отмена";            
        }
        public AddTNViewModel(AddTNView view, TN tn)
        {
            _window = view;
            _window.Title = "Редактирование ТН";
            _window.addBut.Content = "Сохранить";
            _window.clBut.Content = "Отмена";
            TN = tn;
        }
        #endregion

        private void Add(object? obj)
        {
            if (TN.IsValid)
            {
                if (TN.ID != 0)
                {
                    App.DbContext.TNs.Update(TN);
                }
                else
                {
                    App.DbContext.TNs.Add(TN);
                }
                Close(null);
            }
            else
            {
                System.Windows.MessageBox.Show("Введите всю требуемую информацию!", _window.Title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
        }
        private void Close(object? obj) => _window.DialogResult = true;
    }
}
