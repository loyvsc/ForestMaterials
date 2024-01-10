using BuildMaterials.Models;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddAccountViewModel
    {
        public Account Account { get; set; } = new Account();
        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand((sender) => AddMaterial());

        private readonly Window _window = null!;

        public List<Organization> Organizations => App.DbContext.Organizations.ToList();
        public List<Contract> Contracts => App.DbContext.Contracts.ToList();

        public AddAccountViewModel() { }

        public AddAccountViewModel(Window window) : this()
        {
            _window = window;
        }

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void AddMaterial()
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
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Не вся информация была введена!", "Новый счет-фактура", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}