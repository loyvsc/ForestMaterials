using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContactViewModel : ViewModelBase
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

        public ContactType? ContactTypeIndex
        {
            get => Contact.ContactType;
            set
            {
                if (value != null)
                {
                    view.contactText.Visibility = Visibility.Collapsed;
                    Contact.ContactType = (ContactType) (int) value;
                }
            }
        }

        public ICommand AddCommand => new AsyncRelayCommand(AddContact);
        public ICommand CloseCommand => new AsyncRelayCommand(Close);
        #endregion

        #region Constructors
        public AddContactViewModel()
        {
            Contracts = App.DbContext.Contracts.ToList();
            Title = "Добавление контакта";
        }

        public AddContactViewModel(AddContactView view, Organization organization) : this()
        {
            OrganizationID = organization.ID;
            this.view = view;
            Contact = new Contact();
            Title = "Изменение контакта";
        }

        public AddContactViewModel(AddContactView view, Contact contact) : this()
        {
            this.view = view;
            Contact = contact;
            Title = "Изменение контакта";
        }
        #endregion

        private Contact? contact;
        private List<Contract> _contracts;
        private readonly AddContactView view;

        private async Task Close(object? obj = null) => view.DialogResult = false;
        private async Task AddContact(object? obj)
        {
            if (Contact.IsValid)
            {
                AddOrganizationViewModel.Operations.Add(Contact);
                view.DialogResult = true;
            }
            else
            {
                view.ShowDialogAsync("Введите всю требуемую информацию!", Title);
            }
        }
    }
}
