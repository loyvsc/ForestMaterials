using BuildMaterials.BD;
using BuildMaterials.Export;

namespace BuildMaterials.Models
{
    public class Employee : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        [ExportColumnName("Имя")]
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Фамилия")]
        public string? Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Отчество")]
        public string? Pathnetic
        {
            get => pathnetic;
            set
            {
                pathnetic = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Должность")]
        public string? Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Номер телефона")]
        public string? PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }
        [IgnoreProperty]
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        [ExportColumnName("Финансово ответственный")]
        [BooleanValue("Является", "Не является")]
        public bool FinResponsible
        {
            get => finResponsible;
            set
            {
                finResponsible = value;
                OnPropertyChanged();
            }
        }

        [ExportColumnName("Паспортные данные")]
        public Passport Passport
        {
            get => passport;
            set
            {
                passport = value;
                OnPropertyChanged();
            }
        }

        [IgnoreProperty] public bool CanUserAdd
        {
            get => canadd;
            set
            {
                canadd = value;
                OnPropertyChanged();
            }
        }
        [IgnoreProperty] public bool CanUserEdit
        {
            get => canedit;
            set
            {
                canedit = value;
                OnPropertyChanged();
            }
        }
        [IgnoreProperty] public bool CanUserDelete
        {
            get => candel;
            set
            {
                candel = value;
                OnPropertyChanged();
            }
        }
        [IgnoreProperty] public bool IsUserAdmin
        {
            get => isadmin;
            set
            {
                isadmin = value;
                OnPropertyChanged();
                CanUserAdd = value;
                CanUserEdit = value;
                CanUserDelete = value;
            }
        }

        private Passport passport;
        private string? name;
        private string? surname;
        private string? pathnetic;
        private string? position;
        private string? phoneNumber;
        private string password;
        private bool finResponsible = false;

        private bool canadd;
        private bool candel;
        private bool canedit;
        private bool isadmin;

        public Employee()
        {
            ID = 0;
            IsUserAdmin = false;
            Passport = new();
        }

        public Employee(int id, string name, string surName, string pathnetic, string position, string phoneNumber, Passport passport, string password = "", bool finResp = false,
            bool canAdd = false, bool canEdit = false, bool canDel = false, bool isAdmin = false)
        {
            ID = id;
            FinResponsible = finResp;
            Name = name;
            Surname = surName;
            Pathnetic = pathnetic;
            Position = position;
            PhoneNumber = phoneNumber;
            Password = password;
            Passport = passport;

            IsUserAdmin = isAdmin;

            CanUserAdd = canAdd;
            CanUserEdit = canEdit;
            CanUserDelete = canDel;
        }

        public Employee(string position, string password, bool canAdd = false, bool canEdit = false, bool canDel = false, bool isAdmin = false)
        {
            Position = position;
            Password = password;

            IsUserAdmin = isAdmin;
            CanUserAdd = canAdd;
            CanUserEdit = canEdit;
            CanUserDelete = canDel;
        }

        public override string ToString() => FIO;

        [IgnoreProperty]
        public string FIO => $"{Surname} {Name} {Pathnetic}";

        [IgnoreProperty]
        public bool IsValid =>
            Passport.IsValid &&
            Name != string.Empty &&
            Surname != string.Empty &&
            Pathnetic != string.Empty &&
            Position != string.Empty &&
            PhoneNumber != string.Empty &&
            (CanUserAdd != false || CanUserEdit != false || CanUserDelete != false || IsUserAdmin != false);
    }
}