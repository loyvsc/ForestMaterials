using BuildMaterials.BD;
using BuildMaterials.Export;

namespace BuildMaterials.Models
{
    public class Material : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }

        [ExportColumnName("Наименование")]
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Сорт")]
        public string? Sort
        {
            get => sort;
            set
            {
                sort = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Цена")]
        public float Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("НДС")]
        public float NDS
        {
            get => nds;
            set
            {
                nds = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Количество")]
        public float Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Ед. измерения")]
        public string? CountUnits
        {
            get => countUnits;
            set
            {
                countUnits = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Длина")]
        public string? Dlina
        {
            get => dlina;
            set
            {
                dlina = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Ширина")]
        public string? Shirina
        {
            get => shirina;
            set
            {
                shirina = value;
                OnPropertyChanged();
            }
        }
        [ExportColumnName("Дата поставки")]
        public DateTime EnterDate
        {
            get => enterDate;
            set
            {
                enterDate = value;
                OnPropertyChanged();
            }
        }

        private string? sort;
        private string? shirina;
        private string? dlina;
        private string? name;
        private float price;
        private float count;
        private string? countUnits;
        private DateTime enterDate;
        private float nds;

        public Material()
        {
            Name = string.Empty;
            CountUnits = string.Empty;
            Price = 0;
            Count = 0;
            ID = 0;
        }

        public Material(int id)
        {
            ID = id;
        }
          
        public Material(int id, string name, float price, float count, string countUnits, DateTime enterDate, string shirina, string dlina,string sort, float nds)
        {
            ID = id;
            Name = name;
            Price = price;
            Count = count;
            CountUnits = countUnits;
            EnterDate = enterDate;
            Shirina = shirina;
            Dlina = dlina;
            Sort = sort;
            NDS = nds;
        }

        [IgnoreProperty]
        public bool IsValid() => Name != string.Empty &&
            CountUnits != string.Empty && sort!=string.Empty;

        public override string ToString() => Name!;
    }
}