using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public static class Extensions
    {
        public static MySqlDataReader ExecuteMySqlReaderAsync(this MySqlCommand command) => command.ExecuteReader();
    }

    public class AddTTNViewModel : ViewModelBase
    {
        private readonly AddTTNView _window = null!;
        public TTN TTN
        {
            get => ttn;
            set
            {
                ttn = value;
                OnPropertyChanged();
            }
        }

        public int ContractID
        {
            get => TTN.Contract.ID;
            set
            {
                if (value != 0)
                {
                    _window.contractText.Visibility = System.Windows.Visibility.Collapsed;
                    TTN.Contract.ID = value;
                    OnPropertyChanged(nameof(TTN.Contract.ID));
                }
            }
        }

        public int RespEmpl
        {
            get => TTN.ResponseEmployee.ID;
            set
            {
                _window.respText.Visibility = System.Windows.Visibility.Collapsed;
                TTN.ResponseEmployee.ID = value;
                OnPropertyChanged(nameof(TTN.ResponseEmployee.ID));
            }
        }

        public int SdalEmpl
        {
            get => TTN.SdalEmployee.ID;
            set
            {
                if (value != null)
                {
                    _window.sdalText.Visibility = System.Windows.Visibility.Collapsed;
                    TTN.ResponseEmployee.ID = value;
                    OnPropertyChanged(nameof(TTN.SdalEmployee.ID));
                }
            }
        }

        public int Automobile
        {
            get => TTN.Automobile.ID;
            set
            {
                if (value != null)
                {
                    _window.autoText.Visibility = System.Windows.Visibility.Collapsed;
                    TTN.Automobile.ID = value;
                    OnPropertyChanged(nameof(TTN.Automobile.ID));                    
                }
            }
        }

        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);

        public List<Contract> Contracts => App.DbContext.Contracts.ToList();
        public List<Automobile> Automobiles => App.DbContext.Automobiles.ToList();
        public List<Employee> Employees => App.DbContext.Employees.ToList();
        public List<Organization>? CustomersList => App.DbContext.Organizations.ToList();

        private TTN ttn;

        private async Task Close(object? obj = null) => _window.DialogResult = true;

        public AddTTNViewModel(AddTTNView window)
        {
            TTN = new TTN();
            _window = window;
            Title = "Добавление ТТН";
        }

        public AddTTNViewModel(AddTTNView window, Models.TTN ttn)
        {
            _window = window;
            TTN = ttn;
            Title = "Изменение ТТН";
        }

        private async Task AddMaterial(object? obj)
        {
            if (TTN.IsValid)
            {
                try
                {
                    if (TTN.ID != 0)
                    {
                        App.DbContext.TTNs.Update(TTN);
                    }
                    else
                    {
                        App.DbContext.TTNs.Add(TTN);
                    }
                    Close();
                }
                catch (Exception ex)
                {
                    _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message,Title);
                }
            }
            else
            {
                _window.ShowDialogAsync("Введена не вся требуемая информация!", Title);
            }

        }
    }
}