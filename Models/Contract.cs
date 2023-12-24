using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Contract : NotifyPropertyChangedBase, ITable
    {
        private readonly bool UseBD;
        public int ID { get; set; }

        public Organization? Seller
        {
            get => SellerID != null ? App.DbContext.Organizations.ElementAt((int)SellerID) : null;
            set
            {
                if (value != null)
                {
                    SellerID = value.ID;
                    OnPropertyChanged(nameof(Seller));
                    OnPropertyChanged(nameof(SellerID));
                }
            }
        }

        public Organization Buyer
        {
            get => BuyerID != null ? App.DbContext.Organizations.ElementAt((int)BuyerID) : null;
            set
            {
                if (value != null)
                {
                    BuyerID = value.ID;
                    OnPropertyChanged(nameof(Buyer));
                    OnPropertyChanged(nameof(BuyerID));
                }
            }
        }
        public int? SellerID
        {
            get => seller;
            set
            {
                seller = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Contract SET Seller ={value} WHERE ID={ID};");
                }
            }
        }
        public int? BuyerID
        {
            get => buyer;
            set
            {
                buyer = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Contract SET Buyer ={value} WHERE ID={ID};");
                }
            }
        }
        public int MaterialID
        {
            get => matid;
            set
            {
                matid = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Contracts SET MaterialID ={value} WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Material));
                OnPropertyChanged(nameof(MaterialID));
            }
        }
        public Material? Material
        {
            get => App.DbContext.Materials.ElementAt(MaterialID);
            set
            {
                if (value != null)
                {
                    MaterialID = value.ID;
                    OnPropertyChanged(nameof(Material));
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
                    App.DbContext.Query($"UPDATE Contract SET Count ='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Summ));
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
                    App.DbContext.Query($"UPDATE Contract SET CountUnits ='{value}' WHERE ID={ID};");
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
                    App.DbContext.Query($"UPDATE Contract SET Price ='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
            }
        }
        public float Summ => Count * Price;
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Contract SET Date ='{value!.Value.Year}-{value!.Value.Month}-{value!.Value.Day}' WHERE ID={ID};");
                }
            }
        }
        public string? DateInString => Date?.ToShortDateString();

        private DateTime? date;
        private string? countUnits = string.Empty;
        private int? seller = -1;
        private int? buyer = -1;
        private float count = 0;
        private float price = 0;
        private int matid = -1;

        public Contract()
        {
            UseBD = false;
        }

        public Contract(int iD, int? seller, int? buyer, int matid, float count,
            string? countUnits, float price, DateTime? date)
        {
            UseBD = false;
            ID = iD;
            SellerID = seller;
            BuyerID = buyer;
            MaterialID = matid;
            Count = count;
            CountUnits = countUnits;
            Price = price;
            Date = date;
            UseBD = true;
        }

        public override string ToString()
        {
            return $"Договор купли-продажи #{ID} от {DateInString}\nПокупатель: {BuyerID}\nПродавец: {SellerID}\nТовар \"{Material.Name}\" в количестве {Count} {CountUnits}.\nЦена за единицу ({CountUnits}): {Price}.\nСумма: {Summ}.\n\n               {SellerID}\n\n               {BuyerID}";
        }

        public bool IsValid => Date != null;
    }
}