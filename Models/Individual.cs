using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Individual : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public string? Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged();
            }
        }
        public string? Pathnetic
        {
            get => pathnetic;
            set
            {
                pathnetic = value;
                OnPropertyChanged();
            }
        }
        public string? PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }

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

        public string FIO => $"{Surname} {Name} {Pathnetic}";

        public bool IsValid =>
            Passport.IsValid &&
            Name != string.Empty &&
            Surname != string.Empty &&
            Pathnetic != string.Empty &&
            PhoneNumber != string.Empty;
    }
}
