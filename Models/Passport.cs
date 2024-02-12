using BuildMaterials.BD;
using BuildMaterials.Export;

namespace BuildMaterials.Models
{
    public class Passport : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        [ExportColumnName("Идентификационный номер")]
        public string? Number
        {
            get => number;
            set
            {
                if (value == null) return;
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

        [ExportColumnName("Кем выдан")]
        public string? IssuePunkt
        {
            get => ispun;
            set
            {
                ispun = value;
                OnPropertyChanged();
            }
        }

        private DateTime? isdat;
        private string? number;
        private string? ispun;

        public Passport() { }

        public Passport(int iD, string number, DateTime issueDate, string issuePunkt)
        {
            ID = iD;
            Number = number;
            IssueDate = issueDate;
            IssuePunkt = issuePunkt;
        }

        public bool IsValid => Number!=null && BuildMaterials.Extensions.PassportExtensions.CheckPassportNumber(Number) && IssueDate != null;

        public override string ToString() => Number + " " + IssueDate?.ToShortDateString();
    }
}
