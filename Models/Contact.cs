using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public enum ContactType
    {
        None, Email, Phonenumber
    }

    public class Contact : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public bool ForDelete { get; set; } = false;
        public int OrganizationID { get; set; }
        public ContactType ContactType
        {
            get => cType;
            set
            {
                cType = value;
                OnPropertyChanged(nameof(ContactType));
            }
        }
        public string? Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        private ContactType cType;
        private string? text;

        public Contact()
        {
            ID = 0;
        }

        public Contact(int iD, int orgID, ContactType contactType, string? text)
        {
            ID = iD;
            OrganizationID = orgID;
            ContactType = contactType;
            Text = text;
        }

        public bool IsValid => Text !="" &&
            ContactType != 0;
    }
}
