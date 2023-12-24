﻿using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Employee : NotifyPropertyChangedBase, ITable
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
        public string? Position
        {
            get => position;
            set
            {
                position = value;
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
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public bool FinResponsible
        {
            get => finResponsible;
            set
            {
                finResponsible = value;
                OnPropertyChanged();
            }
        }
        public int AccessLevel
        {
            get => accessLevel;
            set
            {
                accessLevel = value;
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
        private string? position;
        private string? phoneNumber;
        private string password;
        private bool finResponsible = false;
        private int accessLevel;

        public string AccessLevelInString => App.DbContext.AccessLevel[AccessLevel];

        public Employee()
        {
            ID = 0;
        }

        public Employee(int id, string name, string surName, string pathnetic, string position, string phoneNumber, Passport passport, string password = "", int accessLevel = 3, bool finResp = false)
        {
            ID = id;
            FinResponsible = finResp;
            Name = name;
            Surname = surName;
            Pathnetic = pathnetic;
            Position = position;
            PhoneNumber = phoneNumber;
            Password = password;
            AccessLevel = accessLevel;
            Passport = passport;
        }

        public Employee(string position, string password, int accessLevel)
        {
            Position = position;
            Password = password;
            AccessLevel = accessLevel;
        }

        public override string ToString() => $"{Surname} {Name} {Pathnetic}";

        public string FIO => $"{Surname} {Name} {Pathnetic}";

        public bool IsValid =>
            Passport.IsValid &&
            Name != string.Empty &&
            Surname != string.Empty &&
            Pathnetic != string.Empty &&
            Position != string.Empty &&
            PhoneNumber != string.Empty;
    }
}