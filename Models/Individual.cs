using BuildMaterials.BD;
using BuildMaterials.Export;

namespace BuildMaterials.Models
{
    public class Individual : NotifyPropertyChangedBase, ITable
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
        [ExportColumnName("Контактный номер телефона")]
        public string? PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }

        [ExportColumnName("Пасспортные данные")]
        public Passport Passport
        {
            get => passport;
            set
            {
                passport = value;
                OnPropertyChanged();
            }
        }

        private Passport passport;
        private string? name;
        private string? surname;
        private string? pathnetic;
        private string? phoneNumber;

        public Individual()
        {
            ID = 0;
            Passport = new Passport();
        }

        public Individual(int id, string name, string surName, string pathnetic, string phoneNumber, Passport passport)
        {
            ID = id;
            Name = name;
            Surname = surName;
            Pathnetic = pathnetic;
            PhoneNumber = phoneNumber;
            Passport = passport;
        }

        [IgnoreProperty]
        public string FIO => $"{Surname} {Name} {Pathnetic}";

        [IgnoreProperty]
        public bool IsValid =>
            Passport.IsValid &&
            Name != string.Empty &&
            Surname != string.Empty &&
            Pathnetic != string.Empty &&
            PhoneNumber != string.Empty;
    }
}
