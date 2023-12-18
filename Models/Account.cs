using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Account : NotifyPropertyChangedBase, ITable
    {
        private readonly bool UseBD;

        public int ID { get; set; }
        public Employee? Seller
        {
            get => SellerID != null ? App.DbContext.Employees.ElementAt((int)SellerID) : null;
            set
            {
                if (value != null)
                {
                    SellerID = value.ID;
                    OnPropertyChanged(nameof(SellerID));
                    OnPropertyChanged(nameof(Seller));
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
                    App.DbContext.Query($"UPDATE Accounts SET Seller={value} WHERE ID={ID};");
                }
            }
        }
        public string? ShipperName
        {
            get => shipperName;
            set
            {
                shipperName = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Accounts SET ShipperName='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ShipperAdress
        {
            get => shipperAdress;
            set
            {
                shipperAdress = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Accounts SET ShipperAdress='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ConsigneeName
        {
            get => consigneeName;
            set
            {
                consigneeName = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Accounts SET ConsigneeName='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ConsigneeAdress
        {
            get => consigneeAdress;
            set
            {
                consigneeAdress = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Accounts SET ConsigneeAdress='{value}' WHERE ID={ID};");
                }
            }
        }
        public Employee? Buyer
        {
            get => BuyerID != null ? App.DbContext.Employees.ElementAt((int)BuyerID) : null;
            set
            {
                if (value != null)
                {
                    BuyerID = value.ID;
                    OnPropertyChanged(nameof(BuyerID));
                    OnPropertyChanged(nameof(Buyer));
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
                    App.DbContext.Query($"UPDATE Accounts SET Buyer={value} WHERE ID={ID};");
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
                    App.DbContext.Query($"UPDATE Accounts SET CountUnits='{value}' WHERE ID={ID};");
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
                    App.DbContext.Query($"UPDATE Accounts SET Count='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(Summ));
                OnPropertyChanged(nameof(TaxSumm));
                OnPropertyChanged(nameof(FinalSumm));
                OnPropertyChanged(nameof(FinalSummAtString));
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
                    App.DbContext.Query($"UPDATE Accounts SET Price='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
                OnPropertyChanged(nameof(TaxSumm));
                OnPropertyChanged(nameof(FinalSumm));
                OnPropertyChanged(nameof(FinalSummAtString));
            }
        }
        public float Summ => Count * Price;
        public float Tax
        {
            get => tax;
            set
            {
                if (value > 100 || value < 0) return;
                tax = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Accounts SET Tax='{value}' WHERE ID={ID};");
                }
                OnPropertyChanged(nameof(Tax));
                OnPropertyChanged(nameof(TaxSumm));
                OnPropertyChanged(nameof(FinalSumm));
                OnPropertyChanged(nameof(FinalSummAtString));
            }
        }
        public float TaxSumm => Summ * (Tax / 100);
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Date SET Date='{value}' WHERE ID={ID};");
                }
            }
        }
        public float FinalSumm => Summ + TaxSumm;
        public string FinalSummAtString => "Итого: " + (Summ + TaxSumm);
        public string? DateInString => Date?.ToShortDateString();
        public int MaterialID
        {
            get => matid;
            set
            {
                matid = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Accounts SET MaterialID ={value} WHERE ID={ID};");
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

        private int matid;
        private DateTime? date;
        private string? countUnits = string.Empty;
        private int? buyer = -1;
        private string? shipperAdress = string.Empty;
        private string? consigneeAdress = string.Empty;
        private string? consigneeName = string.Empty;
        private int? seller = -1;
        private string? shipperName = string.Empty;
        private float count = 0;
        private float price = 0;
        private float tax = 0;

        public Account()
        {
            UseBD = false;
        }

        public Account(int iD, int? seller, string? shipperName, string? shipperAdress, string? consigneeName, string? consigneeAdress, int? buyer, string? countUnits, float count, float price, float tax, DateTime? date, int materialid)
        {
            UseBD = false;
            ID = iD;
            SellerID = seller;
            ShipperName = shipperName;
            ShipperAdress = shipperAdress;
            ConsigneeName = consigneeName;
            ConsigneeAdress = consigneeAdress;
            BuyerID = buyer;
            CountUnits = countUnits;
            Count = count;
            Price = price;
            Tax = tax;
            Date = date;
            MaterialID = materialid;
            UseBD = true;
        }

        public override string ToString()
        {
            return $"Счет №{ID} от {DateInString}\nПродавец: {SellerID}\nПокупатель: {BuyerID}\nГрузоотправитель: {ShipperName} (адрес: {ShipperAdress})\nГрузополучатель: {ConsigneeName} (адрес: {ConsigneeAdress})\nКоличество: {Count} {CountUnits}\nЦена: {Price}\nСумма: {Summ}\nНалоговый сбор: {TaxSumm}\nИтого: {Summ + TaxSumm}";
        }

        public bool IsValid => Date != null
            && MaterialID != -1
            && SellerID != -1
            && ShipperName != string.Empty
            && ShipperAdress != string.Empty
            && ConsigneeName != string.Empty
            && ConsigneeAdress != string.Empty
            && BuyerID != -1
            && CountUnits != string.Empty;
    }
}