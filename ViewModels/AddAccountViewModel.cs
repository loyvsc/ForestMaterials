using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddAccountViewModel : ViewModelBase
    {
        public Account Account { get; set; } = new Account();
        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);

        private readonly AddAccountView _window = null!;

        public List<Organization> Organizations => App.DbContext.Organizations.ToList();
        public List<Contract> Contracts => App.DbContext.Contracts.ToList().Where(x => x.Buyer.ID != 0).ToList();

        public AddAccountViewModel(AddAccountView window)
        {
            _window = window;
            Title = "Добавление счёта-фактуры";
        }

        public DateTime? Date
        {
            get => Account.Date;
            set
            {
                if (value != null)
                {
                    Account.Date = value;
                    _window.dateText.Visibility = Visibility.Collapsed;
                }
            }
        }

        public int? ContractID
        {
            get => Account.Contract.ID;
            set
            {
                if (value != null)
                {
                    Account.Contract.ID = (int) value;
                    _window.contractText.Visibility = Visibility.Collapsed;
                }
            }
        }
        public int? SellerID
        {
            get => Account.Seller.ID;
            set
            {
                if (value != null)
                {
                    Account.Seller.ID = (int)value;
                    _window.sellerText.Visibility = Visibility.Collapsed;
                }
            }
        }
        public int? BuyerID
        {
            get => Account.Buyer.ID;
            set
            {
                if (value != null)
                {
                    Account.Buyer.ID = (int)value;
                    _window.buyerText.Visibility = Visibility.Collapsed;
                }
            }
        }
        
        private async Task Close(object? obj = null) => _window.DialogResult = true;
        private async Task AddMaterial(object? obj)
        {
            if (Account.IsValid)
            {
                if (Account.ID != 0)
                {
                    App.DbContext.Accounts.Update(Account);
                }
                else
                {
                    App.DbContext.Accounts.Add(Account);
                }
                await Close();
            }
            else
            {
                _window.ShowDialogAsync("Не вся информация была введена!", Title);
            }
        }
    }
}