using BuildMaterials.Extensions;
using BuildMaterials.Helpers;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
using System.Windows.Input;

namespace BuildMaterials.ViewModels
{
    public class AddContactViewModel : ViewModelBase
    {
        public PhoneNumberInputHelper PhoneNumberInput { get; set; } = new PhoneNumberInputHelper();
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

        public bool IsPhoneNumber
        {
            get => isPhoneNumber;
            set
            {
                isPhoneNumber = value;
                OnPropertyChanged();
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
                    view.contactText.Visibility = value == ContactType.None ? Visibility.Visible : Visibility.Collapsed;
                    IsPhoneNumber = value == ContactType.Phonenumber;
                    view.emailTextbox.Visibility = IsPhoneNumber ? Visibility.Collapsed : Visibility.Visible;
                    view.phoneTextbox.Visibility = IsPhoneNumber ? Visibility.Visible : Visibility.Collapsed; ;
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

        public AddContactViewModel(AddContactView view, Organization organization)
        {
            Contracts = App.DbContext.Contracts.ToList();
            OrganizationID = organization.ID;
            this.view = view;
            Contact = new Contact();
            Title = "Добавление контакта";            
        }

        public AddContactViewModel(AddContactView view, Contact contact)
        {
            Contracts = App.DbContext.Contracts.ToList();
            this.view = view;
            Contact = contact;
            Title = "Изменение контакта";
            view.contactText.Visibility = Visibility.Collapsed;
            if (Contact.ContactType == ContactType.Phonenumber)
            {
                PhoneNumberInput.Phone = Contact.Text;
            }
        }
        #endregion

        private Contact? contact;
        private List<Contract> _contracts;
        private bool isPhoneNumber;
        private readonly AddContactView view;

        private async Task Close(object? obj = null) => view.DialogResult = false;
        private async Task AddContact(object? obj)
        {
            if (Contact.IsValid)
            {                
                if (Contact.ContactType == ContactType.Phonenumber)
                {
                    Contact.Text = PhoneNumberInput.Phone;
                }                
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
