using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContactViewModel : NotifyPropertyChangedBase
    {
        #region Public propertys
        public Contact? Contact
        {
            get => contact;
            set
            {
                contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }
        public List<Contract> Contracts
        {
            get => _contracts;
            set
            {
                _contracts = value;
                OnPropertyChanged(nameof(Contracts));
            }
        }
        public int OrganizationID { get; set; }

        public ICommand AddCommand => new RelayCommand(AddContact);
        public ICommand CloseCommand => new RelayCommand(Close);
        #endregion

        #region Constructors
        public AddContactViewModel()
        {
            Contracts = App.DbContext.Contracts.ToList();
        }

        public AddContactViewModel(AddContactView view, Organization organization) : this()
        {
            OrganizationID = organization.ID;
            this.view = view;
            Contact = new Contact();
        }

        public AddContactViewModel(AddContactView view, Contact contact) : this()
        {
            this.view = view;
            Contact = contact;
        }
        #endregion

        private Contact? contact;
        private List<Contract> _contracts;
        private readonly AddContactView view;

        private void Close(object? obj = null) => view.DialogResult = true;

        private void AddContact(object? obj)
        {
            if (Contact.IsValid)
            {
                AddOrganizationViewModel.Operations.Add(Contact);
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Введите всю требуемую информацию!", "Редактирование контакта", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
        }
    }
}
