using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddTNViewModel : ViewModelBase
    {
        #region Public props
        public TN? TN { get; set; }

        public List<Contract> ContractsList => App.DbContext.Contracts.ToList();
        public List<Employee> EmployeesList => App.DbContext.Employees.ToList();

        public ICommand AddCommand => new AsyncRelayCommand(Add);
        public ICommand CloseCommand => new AsyncRelayCommand(Close);

        public int? DogovorID
        {
            get => TN.Contract.ID;
            set
            {
                if (value != null)
                {
                    _window.dogovorTxt.Visibility = System.Windows.Visibility.Collapsed;
                    TN.Contract.ID = (int)value;
                }
            }
        }
        public int? RespID
        {
            get => TN.ResponseEmployee.ID;
            set
            {
                if (value != null)
                {
                    _window.respText.Visibility = System.Windows.Visibility.Collapsed;
                    TN.ResponseEmployee.ID = (int)value;
                }
            }
        }
        public int? SdalID
        {
            get => TN.SdalEmployee.ID;
            set
            {
                if (value != null)
                {
                    _window.sdalText.Visibility = System.Windows.Visibility.Collapsed;
                    TN.SdalEmployee.ID = (int)value;
                }
            }
        }
        #endregion

        #region Private vars
        private readonly AddTNView _window;
        #endregion

        #region Constructors
        public AddTNViewModel(AddTNView view)
        {
            TN = new TN();
            _window = view;
            Title = "Добавление ТН";
            _window.addBut.Content = "Добавить";
            _window.clBut.Content = "Отмена";            
        }
        public AddTNViewModel(AddTNView view, TN tn)
        {
            _window = view;
            Title = "Изменение ТН";
            _window.addBut.Content = "Сохранить";
            _window.clBut.Content = "Отмена";
            TN = tn;
        }
        #endregion

        private async Task Add(object? obj)
        {
            if (TN.IsValid)
            {
                try
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
                catch (Exception ex)
                {
                    _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
                }
            }
            else
            {
                _window.ShowDialogAsync("Введите всю требуемую информацию!", Title);
            }
        }
        private async Task Close(object? obj) => _window.DialogResult = true;
    }
}
