using BuildMaterials.BD;
using BuildMaterials.Extensions;
using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows.Input;
using UNPRead;

namespace BuildMaterials.ViewModels
{
    public static class UNPExtensions
    {
        public static Organization UnpToOrganization(this UNPRead.UNP unp)
        {
            unp.FullName ??= string.Empty;
            return new Organization(0, unp.FullName, unp.ShortName, unp.Adress, unp.RegistrationDate, unp.MNSNumber, unp.MNSName, unp.UNPCode, "", "", "", "");
        }
    }

    public class AddOrganizationViewModel : ViewModelBase
    {
        public List<ITable> Operations { get; set; } = new List<ITable>(16);
        public Organization Organization { get; set; }
        public List<Contact> Contacts
        {
            get => organizationContacts;
            set
            {
                organizationContacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }
        public Contact? SelectedContact
        {
            get => cntct;
            set
            {
                cntct = value;
                OnPropertyChanged(nameof(SelectedContact));
            }
        }

        #region Commands
        public ICommand CancelCommand => new AsyncRelayCommand(Close);
        public ICommand AddCommand => new AsyncRelayCommand(AddMaterial);

        public ICommand AddContactCommand => new AsyncRelayCommand(AddContact);
        public ICommand EditContactCommand => new AsyncRelayCommand(EditContact);
        public ICommand DeleteContactCommand => new AsyncRelayCommand(DeleteContact);
        #endregion

        private Contact? cntct;
        private List<Contact> organizationContacts;
        private readonly AddOrganizationView _window = null!;

        #region Constructors
        private AddOrganizationViewModel()
        {
            Contacts = new List<Contact>();
        }
        public AddOrganizationViewModel(AddOrganizationView window) : this()
        {
            Title = "Добавление информации об организации";
            Organization = new Models.Organization();
            _window = window;
        }
        public AddOrganizationViewModel(AddOrganizationView window, Organization organization) : this()
        {
            _window = window;
            Title = "Изменении информации об организации";
            Organization = organization;
            Organization.Contacts = new System.Collections.ObjectModel.ObservableCollection<Contact>(App.DbContext.Contacts.Select("SELECT * FROM CONTACTS WHERE ORGANIZATIONID = " + Organization.ID));
        }
        #endregion

        private async Task AddContact(object? obj)
        {
            AddContactView contact = new AddContactView(Organization,this);
            try
            {
                if (contact.ShowDialog() == true)
                {
                    Contact last = (Contact)Operations.Last();
                    Organization.Contacts.Add(last);
                }
            }
            catch (Exception ex)
            {
                _window.ShowDialogAsync("Произошла ошибка при сохранении изменений...\nОшибка: " + ex.Message, Title);
            }            
        }

        private async Task DeleteContact(object? obj)
        {
            if (SelectedContact == null) return;
            try
            {
                SelectedContact.ForDelete = true;
                Organization.Contacts.Remove(SelectedContact);
                Operations.Add(SelectedContact);
            }
            catch(Exception ex)
            {
                _window.ShowDialogAsync("Произошла ошибка: "+ex.Message, Title);
            }
        }

        private async Task EditContact(object? obj)
        {
            if (SelectedContact != null)
            {
                try
                {
                    AddContactView contact = new AddContactView(SelectedContact, this);
                    if (contact.ShowDialog() == true)
                    {
                        Contact last = (Contact)Operations.Last();
                        Organization.Contacts[Organization.Contacts.IndexOf(last)] = last;
                    }
                }
                catch(Exception ex)
                {
                    _window.ShowDialogAsync("Произошла ошибка: " + ex.Message, Title);
                }
            }            
        }

        private async Task Close(object? obj = null) => _window.DialogResult = true;

        private async Task CheckOperations()
        {
            foreach (var item in Operations)
            {
                switch (item)
                {
                    case Contact contact:
                        {
                            if (contact.ForDelete)
                            {
                                App.DbContext.Contacts.Remove(contact);
                                break;
                            }
                            if (contact.ID != 0)
                            {
                                App.DbContext.Contacts.Update(contact);
                            }
                            else
                            {
                                contact.OrganizationID = Organization.ID;
                                App.DbContext.Contacts.Add(contact);
                            }
                            break;
                        }
                    case Organization organization:
                        {
                            if (organization.ForDelete)
                            {
                                App.DbContext.Organizations.Remove(organization);
                                break;
                            }
                            if (organization.ID != 0)
                            {
                                App.DbContext.Organizations.Update(organization);
                            }
                            else
                            {
                                App.DbContext.Organizations.Add(organization);
                            }
                            break;
                        }
                }
            }
        }

        private async Task AddMaterial(object? obj)
        {
            if (Organization.IsValid)
            {
                UNPReader reader = new UNPReader();
                Organization org;
                try
                {
                    org = reader.GetInfoByUNP(Organization.UNP).UnpToOrganization();
                }
                catch (ArgumentException nEx)
                {
                    _window.ShowDialogAsync(nEx.Message, Title);
                    return;
                }
                org.ID = Organization.ID;
                org.CBU = Organization.CBU;
                org.RascSchet = Organization.RascSchet;
                org.BIK = Organization.BIK;
                org.CurrentSchet = Organization.CurrentSchet;

                try
                {
                    if (org.ID != 0)
                    {
                        App.DbContext.Organizations.Update(org);
                    }
                    else
                    {
                        App.DbContext.Organizations.Add(org);
                        Organization = App.DbContext.Organizations.Select("SELECT * FROM sellers WHERE ID = (SELECT MAX(ID) FROM SELLERS)")[0];
                    }
                }
                catch (Exception ex)
                {
                    _window.ShowDialogAsync("Произошла ошибка: " + ex.Message, Title);
                    return;
                }
                await CheckOperations();
                Close();
            }
            else
            {
                _window.ShowDialogAsync("Введите УНП!", Title);
            }
        }
    }
}