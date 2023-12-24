using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Organization : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public bool ForDelete { get; set; } = false;
        public string? UNP
        {
            get => unp;
            set
            {
                unp = value;
                OnPropertyChanged(nameof(UNP));
            }
        }
        public string? CompanyName
        {
            get => companyName;
            set
            {
                companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }
        public string? ShortCompamyName
        {
            get => shtrcmpname;
            set
            {
                shtrcmpname = value;
                OnPropertyChanged(nameof(ShortCompamyName));
            }
        }
        public string? Adress
        {
            get => adress;
            set
            {
                adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }
        public DateTime? RegistrationDate
        {
            get => regdat;
            set
            {
                regdat = value;
                OnPropertyChanged(nameof(RegistrationDate));
            }
        }
        public string? RegistrationDateInString => RegistrationDate?.ToString("d");
        public string? MNSNumber
        {
            get => msnnum;
            set
            {
                msnnum = value;
                OnPropertyChanged(nameof(MNSNumber));
            }
        }
        public string? MNSName
        {
            get => msnname;
            set
            {
                msnname = value;
                OnPropertyChanged(nameof(MNSName));

            }
        }
        public string? RascSchet
        {
            get => raschScht;
            set
            {
                raschScht = value;
                OnPropertyChanged(nameof(RascSchet));
            }
        }
        public string? CBU
        {
            get => cbu;
            set
            {
                cbu = value;
                OnPropertyChanged(nameof(CBU));
            }
        }
        public List<Contact> Contacts
        {
            get => contacts;
            set
            {
                contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }        

        private List<Contact> contacts;
        private string? cbu;
        private string? raschScht;
        private string? msnname;
        private string? msnnum;
        private string? shtrcmpname;
        private string? companyName;
        private string? adress;
        private string? unp;
        private DateTime? regdat;

        public Organization()
        {
            UNP = "";
            Contacts = new List<Contact>();
        }

        public Organization(int iD, string? companyName, string? shortCompanyName, string? adress, DateTime? regDate, string? mnsNum, string? mnsName, string? uNP, string? rasch, string? cbu, bool initContacts = false)
        {
            ID = iD;
            CompanyName = companyName;
            Adress = adress;
            ShortCompamyName = shortCompanyName;
            RegistrationDate = regDate;
            MNSName = mnsName;
            MNSNumber = mnsNum;
            UNP = uNP;
            RascSchet = rasch;
            CBU = cbu;
            if (initContacts)
            {
                Contacts = App.DbContext.Contacts.Select("SELECT * FROM CONTACTS WHERE ORGANIZATIONID = " + ID);
            }
            else
            {
                Contacts = new List<Contact>();
            }
        }

        public override string ToString() => CompanyName!;
    }
}