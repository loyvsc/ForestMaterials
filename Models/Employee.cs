using BuildMaterials.BD;
using System.Text.RegularExpressions;

namespace BuildMaterials.Models
{
    public partial class Employee : ITable
    {
        private readonly bool UseBD;
        public int ID { get; set; }
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET Name ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? SurName
        {
            get => surname;
            set
            {
                surname = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET SurName ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Pathnetic
        {
            get => pathnetic;
            set
            {
                pathnetic = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET Pathnetic ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Position
        {
            get => position;
            set
            {
                position = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET Position ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET PhoneNumber ='{value}' WHERE ID={ID};");
                }
            }
        }
        public int Password
        {
            get => password;
            set
            {
                password = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET Password ='{value}' WHERE ID={ID};");
                }
            }
        }

        public string? DateInString
        {
            get => PassportIssueDate?.ToShortDateString();
            set
            {
                DateTime date;
                if (DateTime.TryParse(value.Trim(), out date))
                {
                    PassportIssueDate = date;
                }
            }
        }
        public bool FinResponsible
        {
            get => finResponsible;
            set
            {
                finResponsible = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET FinResponsible = {value} WHERE ID={ID};");
                }
            }
        }
        public int AccessLevel
        {
            get => accessLevel;
            set
            {
                accessLevel = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employees SET AccessLevel ={value} WHERE ID={ID};");
                }
            }
        }

        public string PassportNumber
        {
            get => passportNumber;
            set
            {
                if (CheckPassportNumber(value) == false)
                {
                    return;
                }
                passportNumber = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employee SET PassportNumber ='{value}' WHERE ID = {ID};");
                }
            }
        }

        private bool CheckPassportNumber(string passportNumber)
        {
            const string pattern = @"^[A-Z]{2}\d{7}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(passportNumber);
        }

        public DateTime? PassportIssueDate
        {
            get => passportIssueDate;
            set
            {
                passportIssueDate = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Employee SET PassportIssueDate ='{value.Value.ToMySQLDate()}' WHERE ID = {ID};");
                }
            }
        }

        private DateTime? passportIssueDate;
        private string passportNumber = string.Empty;
        private string? name;
        private string? surname;
        private string? pathnetic;
        private string? position;
        private string? phoneNumber;
        private int password;
        private bool finResponsible = false;
        private int accessLevel;

        public string AccessLevelInString => App.DbContext.AccessLevel[AccessLevel];

        public Employee()
        {
            UseBD = false;
        }

        public Employee(int id, string name, string surName, string pathnetic, string position, string phoneNumber, string passportNumber, DateTime issueDate, int password = 0, int accessLevel = 3, bool finResp = false)
        {
            ID = id;
            UseBD = false;
            FinResponsible = finResp;
            Name = name;
            SurName = surName;
            Pathnetic = pathnetic;
            Position = position;
            PhoneNumber = phoneNumber;
            Password = password;
            AccessLevel = accessLevel;
            PassportIssueDate = issueDate;
            PassportNumber = passportNumber;
            UseBD = true;
        }

        public Employee(string position, int password, int accessLevel)
        {
            UseBD = false;
            Position = position;
            Password = password;
            AccessLevel = accessLevel;
        }

        public string Print()
        {
            string text = $"Сотрудник №{ID}\nФ.И.О.: {SurName} {Name} {Pathnetic}\nДолжность: {Position}\nНомер телефона: {PhoneNumber}\n";
            if (FinResponsible)
            {
                text += "Материально ответственный сотрудник";
            }
            else
            {
                text += "Не является материально ответственным";
            }
            return text;
        }

        public override string ToString() => $"{SurName} {Name} {Pathnetic}";

        public string FIO => $"{SurName} {Name} {Pathnetic}";

        public bool IsValid =>
            PassportIssueDate != DateTime.Now &&
            PassportNumber != string.Empty &&
            Name != string.Empty &&
            SurName != string.Empty &&
            Pathnetic != string.Empty &&
            Position != string.Empty &&
            PhoneNumber != string.Empty;
    }
}