using BuildMaterials.BD;
using BuildMaterials.Export;
using System.Collections.ObjectModel;

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
                unp = value?.Trim();
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
                msnnum = value?.Trim();
                OnPropertyChanged(nameof(MNSNumber));
            }
        }
        [ExportColumnName("Наименование МНС")]
        public string? MNSName
        {
            get => msnname;
            set
            {
                msnname = value?.Trim();
                OnPropertyChanged(nameof(MNSName));

            }
        }
        [ExportColumnName("Расчетный счет")]
        public string? RascSchet
        {
            get => raschScht;
            set
            {
                raschScht = value?.Trim();
                OnPropertyChanged(nameof(RascSchet));
            }
        }
        [ExportColumnName("ЦБУ")]
        public string? CBU
        {
            get => cbu;
            set
            {
                cbu = value?.Trim();
                OnPropertyChanged(nameof(CBU));
            }
        }
        [ExportColumnName("Контакты")]
        public ObservableCollection<Contact> Contacts
        {
            get => contacts;
            set
            {
                contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }
        [ExportColumnName("Текущий счет")]
        public string? CurrentSchet
        {
            get => curScht;
            set
            {
                curScht = value?.Trim();
                OnPropertyChanged();
            }
        }
        [ExportColumnName("БИК")]
        public string? BIK
        {
            get => bik;
            set
            {
                if (value.Length < 11)
                {
                    bik = value?.Trim();
                    OnPropertyChanged();
                }
            }
        }

        #region Private vars
        private ObservableCollection<Contact> contacts;
        private string? cbu;
        private string? raschScht;
        private string? bik;
        private string? curScht;
        private string? msnname;
        private string? msnnum;
        private string? shtrcmpname;
        private string? companyName;
        private string? adress;
        private string? unp;
        private DateTime? regdat;
        #endregion

        public bool IsValid => UNP != string.Empty && BIK != string.Empty && CurrentSchet != string.Empty && RascSchet != string.Empty &&
            MNSNumber != string.Empty && MNSName != string.Empty;

        public Organization()
        {
            UNP = "";
            Contacts = new ObservableCollection<Contact>();
        }

        public Organization(int iD, string? companyName, string? shortCompanyName, string? adress, DateTime? regDate, string? mnsNum, string? mnsName, string? uNP, string? rasch, string? cbu, string? bik, string? currentSchet,ObservableCollection<Contact> contacts = null!)
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
            Contacts = contacts != null ? contacts : new ObservableCollection<Contact>();
            CurrentSchet = currentSchet;
            BIK = bik;
        }

        public override string ToString() => CompanyName;
    }
}