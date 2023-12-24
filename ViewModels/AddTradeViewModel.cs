using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddTradeViewModel : NotifyPropertyChangedBase
    {
        public Trade Trade
        {
            get => trade;
            set
            {
                trade = value;
                OnPropertyChanged(nameof(Trade));
            }
        }
        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());
        public List<Employee> SellersFIO => App.DbContext.Employees.ToList();
        public List<Material> Materials => App.DbContext.Materials.ToList();
        public List<PayType> PayTypesList => App.DbContext.PayTypes.ToList();

        public string MaxCountValue
        {
            get => _maxCountValue;
            set
            {
                _maxCountValue = value;
                OnPropertyChanged("MaxCountValue");
            }
        }

        #region Private vars
        private Trade? trade;
        private readonly Window _window = null!;
        private string _maxCountValue = string.Empty;
        #endregion

        #region Constructors
        public AddTradeViewModel()
        {
            Trade = new Trade();
        }

        public AddTradeViewModel(Window window) : this()
        {
            _window = window;
        }
        public AddTradeViewModel(Window window, Models.Trade trade)
        {
            _window = window;
            Trade = trade;
        }
        #endregion

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void AddMaterial()
        {
            if (Trade.Count > Trade.Material.Count)
            {
                System.Windows.MessageBox.Show("Продано больше, чем в наличии!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Trade.Seller == null || Trade.Material == null)
            {
                System.Windows.MessageBox.Show("Введите всю требуемую информацию!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }                
        
            if (Trade.IsValid)
            {
                if (Trade.ID != 0)
                {
                    try
                    {
                        App.DbContext.Trades.Update(Trade);
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("При сохранении изменений произошла ошибка!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Close();
                    return;
                }
                else
                {
                    App.DbContext.Trades.Add(Trade);
                    App.DbContext.Query($"UPDATE Materials SET COUNT = COUNT-{Trade.Count} WHERE id = {Trade.Material.ID};");
                    Close();
                    return;
                }
            }
            System.Windows.MessageBox.Show("Не вся информация была введена!", "Товарооборот", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}