using BuildMaterials.Models;
using BuildMaterials.Views;
using System.Windows;
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

    public class AddOrganizationViewModel : NotifyPropertyChangedBase
    {
        public static List<object> Operations { get; set; } = new List<object>(16);
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
        public ICommand CancelCommand => new RelayCommand(Close);
        public ICommand AddCommand => new RelayCommand(AddMaterial);

        public ICommand AddContactCommand => new RelayCommand(AddContact);
        public ICommand EditContactCommand => new RelayCommand(EditContact);
        public ICommand DeleteContactCommand => new RelayCommand(DeleteContact);
        #endregion

        private Contact? cntct;
        private List<Contact> organizationContacts;
        private readonly Window _window = null!;

        #region Constructors
        public AddOrganizationViewModel()
        {
            Organization = new Models.Organization();
            Operations = new List<object>(16);
            Contacts = new List<Contact>();
        }
        public AddOrganizationViewModel(Window window) : this()
        {
            _window = window;
        }
        public AddOrganizationViewModel(Window window, Organization organization)
        {
            _window = window;
            Organization = organization;
            Organization.Contacts = App.DbContext.Contacts.Select("SELECT * FROM CONTACTS WHERE ORGANIZATIONID = " + Organization.ID);
        }
        #endregion

        private void AddContact(object? obj)
        {
            AddContactView contact = new AddContactView(Organization);
            if (contact.ShowDialog() == true)
            {
                Contact last = (Contact)Operations.Last();
                Organization.Contacts.Add(last);
                Organization.Contacts = Organization.Contacts.ToList();
            }
        }

        private void DeleteContact(object? obj)
        {
            if (SelectedContact == null) return;
            SelectedContact.ForDelete = true;
            Organization.Contacts.Remove(SelectedContact);
            Operations.Add(SelectedContact);

            var items = Organization.Contacts.ToList();
            Organization.Contacts = items;
        }

        private void EditContact(object? obj)
        {
            if (SelectedContact != null)
            {
                AddContactView contact = new AddContactView(SelectedContact);
                if (contact.ShowDialog() == true)
                {
                    Contact last = (Contact)Operations.Last();
                    Organization.Contacts[Organization.Contacts.FindIndex(x => x.ID == last.ID)] = last;
                    Organization.Contacts = Organization.Contacts.ToList();
                }
            }
        }

        private void Close(object? obj = null) => _window.DialogResult = true;

        private void CheckOperations()
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

        private void AddMaterial(object? obj)
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
                    System.Windows.MessageBox.Show(nEx.Message, "Ошибка при получении информации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                org.ID = Organization.ID;
                org.CBU = Organization.CBU;
                org.RascSchet = Organization.RascSchet;
                org.BIK = Organization.BIK;
                org.CurrentSchet = Organization.CurrentSchet;

                if (org.ID != 0)
                {
                    try
                    {
                        App.DbContext.Organizations.Update(org);
                    }
                    catch
                    {
                        System.Windows.MessageBox.Show("Произошла ошибка при сохранении изменений!", "Ошибка при получении информации", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    App.DbContext.Organizations.Add(org);
                    Organization = App.DbContext.Organizations.Select("SELECT * FROM sellers WHERE ID = (SELECT MAX(ID) FROM SELLERS)")[0];
                }
                CheckOperations();
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Введите УНП!", "Добавление поставщика", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}