using BuildMaterials.BD;
using BuildMaterials.Export;
using System.Numerics;

namespace BuildMaterials.Models
{
    public class Passport : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        [ExportColumnName("Номер пасспорта")]
        public string? Number
        {
            get => number;
            set
            {
                if (BuildMaterials.Extensions.PassportExtensions.CheckPassportNumber(value))
                {
                    number = value;
                    OnPropertyChanged();
                }
            }
        }

        [ExportColumnName("Дата выдачи")]
        public DateTime? IssueDate
        {
            get => isdat;
            set
            {
                isdat = value;
                OnPropertyChanged();
            }
        }

        public Passport() { }

        public Passport(int iD, string number, DateTime issueDate)
        {
            ID = iD;
            Number = number;
            IssueDate = issueDate;
        }

        public bool IsValid => Number != null && IssueDate != null;

        public override string ToString() => Number + " " + IssueDate?.ToShortDateString();

        private DateTime? isdat;
        private string? number;
    }
}
