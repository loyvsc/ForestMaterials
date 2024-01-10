using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class TTN : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }
        public Automobile? Automobile
        {
            get => auto;
            set
            {
                auto = value;
                OnPropertyChanged();
            }
        }
        public string? Driver
        {
            get => driver;
            set
            {
                driver = value;
                OnPropertyChanged();
            }
        }
        public Organization? TransitionBuyer
        {
            get => transBuy;
            set
            {
                transBuy = value;
                OnPropertyChanged();
            }
        }
        public Contract? Contract
        {
            get => contract;
            set
            {
                contract = value;
                OnPropertyChanged();
            }
        }
        public string? PogruzkaMethod
        {
            get => pogMet;
            set
            {
                pogMet = value;
                OnPropertyChanged();
            }
        }
        public Employee? SdalEmployee
        {
            get => sdal;
            set
            {
                sdal = value;
                OnPropertyChanged();
            }
        }
        public Employee? ResponseEmployee
        {
            get => razreshil;
            set
            {
                razreshil = value;
                OnPropertyChanged();
            }
        }
        public string? AdresPogruzki
        {
            get => pogr;
            set
            {
                pogr = value;
                OnPropertyChanged();
            }
        }
        public string? AdresRazgruzki
        {
            get => razgr;
            set
            {
                razgr = value;
                OnPropertyChanged();
            }
        }

        #region Private vars
        private string? pogr;
        private string? razgr;
        private Employee? razreshil;
        private Employee? sdal;
        private string? pogMet;
        private Organization? transBuy;
        private DateTime? date;
        private Automobile? auto;
        private string? driver;
        private Contract? contract;
        #endregion

        public TTN()
        {
            Automobile = new Automobile();
            TransitionBuyer = new Organization();
            Contract = new Contract();
            SdalEmployee = new Employee();
            ResponseEmployee = new Employee();
            ID = 0;
            Driver = string.Empty;
            PogruzkaMethod = string.Empty;
            Date = DateTime.Now;
        }

        public TTN(int id, Contract? contract, string? driver, Automobile? automobile, DateTime? date, string? pogMethod, Employee? sdalEmployee, 
            Employee? responseEmployee, string? adressPogruzki, string? adressRazgruzki)
        {
            ID = id;
            Contract = contract;
            Driver = driver;
            Automobile = automobile;
            Date = date;
            PogruzkaMethod = pogMethod;
            SdalEmployee = sdalEmployee;
            ResponseEmployee = responseEmployee;
            AdresPogruzki = adressPogruzki;
            AdresRazgruzki = adressRazgruzki;
        }

        public bool IsValid => Date != null && Contract.ID != 0 && Driver != string.Empty && Automobile.ID != 0
            && PogruzkaMethod != string.Empty && SdalEmployee.ID != 0 && ResponseEmployee.ID != 0
            && Driver != string.Empty;
    }
}