using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Material : ITable
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
                    App.DbContext.Query($"UPDATE Materials SET Name = '{value}' WHERE ID = {ID};");
                }                
            }
        }
        public string? Manufacturer
        {
            get => manufacturer;
            set
            {
                manufacturer = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Materials SET Manufacturer = '{value}' WHERE ID = {ID};");
                }                
            }
        }
        public float Price
        {
            get => price;
            set
            {
                price = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Materials SET Price = '{value}' WHERE ID = {ID};");
                }
            }
        }
        public float Count
        {
            get => count;
            set
            {
                count = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Materials SET Count = '{value}' WHERE ID = {ID};");
                }
            }
        }
        public string? CountUnits
        {
            get => countUnits;
            set
            {
                countUnits = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Materials SET CountUnits = '{value}' WHERE ID = {ID};");
                }
            }
        }
        public DateTime EnterDate
        {
            get => enterDate;
            set
            {
                enterDate = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Materials SET EnterDate = '{value}' WHERE ID = {ID};");
                }
            }
        }
        public string EnterDateAsString
        {
            get => App.DbContext.Materials.Select($"SELECT * FROM MATERIALS WHERE ID = " + ID)[0].EnterDate.ToShortDateString();
            set
            {
                DateTime date;
                if (DateTime.TryParse(value.Trim(),out date))
                {
                    EnterDate = date;
                }
            }
        }

        private string? name;
        private string? manufacturer;
        private float price;
        private float count;
        private string? countUnits;
        private DateTime enterDate;

        public string AsString()
        {
            return $"Материал #{ID}\nНаименование: {Name}\nПроизводитель: {Manufacturer}\nПоступление {EnterDate.Date.ToShortDateString()}\nКоличетсво: {Count} {CountUnits}\nСтоимость: {Price}";
        }

        public Material()
        {
            UseBD = false;
            name = string.Empty;
            Manufacturer = string.Empty;
            CountUnits = string.Empty;
            Price = 0;
            Count = 0;
        }

        public Material(int id)
        {
            UseBD = false;
            ID = id;
        }

        public Material(int id, string name, string manufacturer, float price, float count, string countUnits, DateTime enterDate)
        {
            UseBD = true;
            ID = id;
            this.name = name;
            this.manufacturer = manufacturer;
            this.price = price;
            this.count = count;
            this.countUnits = countUnits;
            this.enterDate = enterDate;
        }

        public bool IsValid => Name != string.Empty &&
            Manufacturer != string.Empty &&
            CountUnits != string.Empty;

        public override string ToString()
        {
            return Name!;
        }
    }
}