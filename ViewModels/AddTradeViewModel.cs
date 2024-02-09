using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddTradeViewModel : ViewModelBase
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
        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);
        public List<Employee> SellersFIO => App.DbContext.Employees.ToList();
        public List<Material> Materials => App.DbContext.Materials.ToList();
        public List<PayType> PayTypesList => App.DbContext.PayTypes.ToList();

        public DateTime? Date
        {
            get => Trade.Date;
            set
            {
                if (value != null)
                {
                    _window.dateText.Visibility = Visibility.Collapsed;
                    Trade.Date = value.Value;
                }
            }
        }

        public int? SelectedMaterialID
        {
            get => Trade.MaterialID;
            set
            {
                if (value != null)
                {
                    _window.matText.Visibility = Visibility.Collapsed;
                    Trade.MaterialID = value;
                    OnPropertyChanged(nameof(Trade.MaterialID));
                }
            }
        }

        public int? SellerID
        {
            get => Trade.SellerID;
            set
            {
                if (value != null)
                {
                    Trade.SellerID = value;
                    _window.selText.Visibility = Visibility.Collapsed;
                }
            }
        }

        public int? PayTypeID
        {
            get => Trade.PayTypeID;
            set
            {
                if (value != null)
                {
                    Trade.PayTypeID = value;
                    _window.payText.Visibility = Visibility.Collapsed;
                }
            }
        }

        #region Private vars
        private Trade? trade;
        private readonly AddTradeView _window = null!;
        private string _maxCountValue = string.Empty;
        #endregion

        #region Constructors

        public AddTradeViewModel(AddTradeView window)
        {
            Trade = new Trade();            
            _window = window;
            Title = "Добавление информации о товарообороте";
        }
        public AddTradeViewModel(AddTradeView window, Models.Trade trade)
        {
            _window = window;
            Trade = trade;
            Title = "Изменение информации о товарообороте";
        }
        #endregion

        private async Task Close(object? obj = null) => _window.DialogResult = true;

        private async Task AddMaterial(object? obj)
        {
            if (Trade.Count > Trade.Material.Count)
            {
                _window.ShowDialogAsync("Продано больше, чем в наличии!", Title);
                return;
            }
            if (Trade.Seller == null || Trade.Material == null)
            {
                _window.ShowDialogAsync("Введите всю требуемую информацию!", Title);
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
                    catch (Exception ex)
                    {
                        _window.ShowDialogAsync(Title, "При сохранении изменений произошла ошибка: " + ex.Message);
                        return;
                    }
                    Close();
                }
                else
                {
                    App.DbContext.Trades.Add(Trade);
                    Close();
                }
            }
            else
            {
                _window.ShowDialogAsync(Title, "Введите всю требуемую информацию!");
            }
        }
    }
}