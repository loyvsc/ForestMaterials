using BuildMaterials.BD;
using BuildMaterials.Export;

namespace BuildMaterials.Models
{
    public class Organization : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        [IgnoreProperty]
        public bool ForDelete { get; set; } = false;
        [ExportColumnName("УНП")]
        public string? UNP
        {
            get => unp;
            set
            {
                unp = value;
                OnPropertyChanged(nameof(UNP));
            }
        }
        [ExportColumnName("Полное наименование компании")]
        public string? CompanyName
        {
            get => companyName;
            set
            {
                companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }
        [ExportColumnName("Краткое наименование компании")]
        public string? ShortCompamyName
        {
            get => shtrcmpname;
            set
            {
                shtrcmpname = value;
                OnPropertyChanged(nameof(ShortCompamyName));
            }
        }
        [ExportColumnName("Адрес")]
        public string? Adress
        {
            get => adress;
            set
            {
                adress = value;
                OnPropertyChanged(nameof(Adress));
            }
        }
        [ExportColumnName("Дата регистрации")]
        public DateTime? RegistrationDate
        {
            get => regdat;
            set
            {
                regdat = value;
                OnPropertyChanged(nameof(RegistrationDate));
            }
        }
        [IgnoreProperty]
        public string? RegistrationDateInString => RegistrationDate?.ToString("d");
        [ExportColumnName("Номер МНС")]
        public string? MNSNumber
        {
            get => msnnum;
            set
            {
                msnnum = value;
                OnPropertyChanged(nameof(MNSNumber));
            }
        }
        [ExportColumnName("Наименование МНС")]
        public string? MNSName
        {
            get => msnname;
            set
            {
                msnname = value;
                OnPropertyChanged(nameof(MNSName));

            }
        }
        [ExportColumnName("Расчетный счет")]
        public string? RascSchet
        {
            get => raschScht;
            set
            {
                raschScht = value;
                OnPropertyChanged(nameof(RascSchet));
            }
        }
        [ExportColumnName("ЦБУ")]
        public string? CBU
        {
            get => cbu;
            set
            {
                cbu = value;
                OnPropertyChanged(nameof(CBU));
            }
        }
        [ExportColumnName("Контакты")]
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

        public Organization(int iD, string? companyName, string? shortCompanyName, string? adress, DateTime? regDate, string? mnsNum, string? mnsName, string? uNP, string? rasch, string? cbu, List<Contact> contacts = null!)
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
            if (contacts != null) Contacts = contacts;
            else Contacts = new List<Contact>();
        }

        public override string ToString() => CompanyName!;
    }
}