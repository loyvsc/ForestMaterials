using BuildMaterials.BD;
using BuildMaterials.Extensions;

namespace BuildMaterials.Models
{
    public class Automobile : NotifyPropertyChangedBase, ITable
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

        public string? RegistrationNumber
        {
            get => regNum;
            set
            {
                if (value.IsAutomobileRegistrationNumber())
                {
                    regNum = value;
                    OnPropertyChanged();
                }
            }
        }

        public Automobile()
        {
            ID = 0;
            Name = string.Empty;
            RegistrationNumber = string.Empty;
        }

        public Automobile(int id, string? name, string? regNum)
        {
            ID = id;
            Name = name;
            RegistrationNumber = regNum;
        }

        public bool IsValid => Name != string.Empty && RegistrationNumber != string.Empty;

        private string? regNum;
        private string? name;

        public override string ToString() => $"{Name} №{RegistrationNumber}";
    }
}
