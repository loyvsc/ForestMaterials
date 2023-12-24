using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class TTN : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public string? Shipper
        {
            get => shipper;
            set
            {
                shipper = value;
                OnPropertyChanged();
            }
        }
        public string? Consignee
        {
            get => consignee;
            set
            {
                consignee = value;
                OnPropertyChanged();
            }
        }
        public string? Payer
        {
            get => payer;
            set
            {
                payer = value;
                OnPropertyChanged();
            }
        }
        public float Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Summ));
            }
        }
        public float? Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
            }
        }
        public int? MaterialID
        {
            get => matid;
            set
            {
                matid = value;
                OnPropertyChanged(nameof(MaterialID));
                OnPropertyChanged(nameof(Material));
            }
        }
        public Material? Material
        {
            get => MaterialID != null ? App.DbContext.Materials.ElementAt((int)MaterialID) : null;
            set
            {
                if (value != null)
                {
                    MaterialID = value.ID;
                    OnPropertyChanged(nameof(MaterialID));
                    OnPropertyChanged(nameof(Material));
                }
            }
        }
        public string? CountUnits
        {
            get => countUnits;
            set
            {
                countUnits = value;
                OnPropertyChanged();
            }
        }
        public float Weight
        {
            get => weight;
            set
            {
                weight = value;
                OnPropertyChanged();
            }
        }
        public float? Summ => Count * Price;
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        private string? shipper = string.Empty;
        private string? consignee = string.Empty;
        private string? payer = string.Empty;
        private int? matid = -1;
        private string? countUnits = string.Empty;
        private float weight = 0;
        private float count = 0;
        private float? price = 0;
        private DateTime? date;

        public string? DateInString
        {
            get => Date?.ToShortDateString();
            set
            {
                if (value?.Trim() != string.Empty)
                {
                    Date = Convert.ToDateTime(value);
                }
            }
        }

        public TTN() { }
        public TTN(int iD, string? shipper, string? consignee, string? payer, float count, float? price, int? matid, string? countUnit, float weight, DateTime? date)
        {
            ID = iD;
            Shipper = shipper;
            Consignee = consignee;
            Payer = payer;
            Count = count;
            Price = price;
            MaterialID = matid;
            CountUnits = countUnit;
            Weight = weight;
            Date = date;
        }

        public override string ToString()
        {
            return $"ТТН от {DateInString}\nГрузоотправитель: {Shipper}\nГрузополучатель: {Consignee}\nПлательщик: {Payer}\nМатериал: {MaterialID}\nКоличество: {Count} {CountUnits}\nЦена: {Price}\nСумма: {Summ}";
        }

        public bool IsValid =>
            Shipper != string.Empty &&
            Consignee != string.Empty &&
            Payer != string.Empty &&
            MaterialID != -1 &&
            CountUnits != string.Empty &&
            Date != null;
    }
}