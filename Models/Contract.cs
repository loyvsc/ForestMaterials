using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Contract : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }

        public Organization? Seller
        {
            get => seller;
            set
            {
                seller = value;
                OnPropertyChanged(nameof(Seller));
            }
        }
        public Organization? Buyer
        {
            get => buyer;
            set
            {
                buyer = value;
                OnPropertyChanged(nameof(Buyer));
            }
        }
        public Individual? Individual
        {
            get => ind;
            set
            {
                ind = value;
                OnPropertyChanged();
            }
        }
        public int SellerID
        {
            get => sellerid;
            set
            {
                sellerid = value;
                if (value != 0)
                    Seller = App.DbContext.Organizations.ElementAt(value);
                OnPropertyChanged(nameof(Seller));
            }
        }
        public int BuyerID
        {
            get => buyerid;
            set
            {
                buyerid = value;
                if (value != 0)
                    Buyer = App.DbContext.Organizations.ElementAt(value);
                OnPropertyChanged(nameof(BuyerID));
            }
        }
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        public List<ContractMaterial> Materials
        {
            get => mats;
            set
            {
                mats = value;
                OnPropertyChanged();
            }
        }

        public string? LogisiticsType
        {
            get => logisticsType;
            set
            {
                logisticsType = value;
                OnPropertyChanged();
            }
        }

        #region Private vars
        private Individual? ind;
        private string? logisticsType;
        private List<ContractMaterial> mats;
        private DateTime? date;
        private Organization? seller;
        private Organization? buyer;
        private int sellerid;
        private int buyerid;
        #endregion

        #region Constructors
        public Contract()
        {
            Materials = new List<ContractMaterial>();
            SellerID = 0;
            BuyerID = 0;
        }

        public Contract(int iD, Organization? seller, Organization? buyer, DateTime? date, List<ContractMaterial> materials,
            string? logisiticsType, Individual? individual)
        {
            ID = iD;
            Materials = materials;
            Seller = seller;
            SellerID = seller.ID;
            Buyer = buyer;
            BuyerID = buyer.ID;
            Date = date;
            LogisiticsType = logisiticsType;
            Individual = individual;
        }
        #endregion

        public bool IsValid => Date != null && Materials.Count > 0 && SellerID != 0 && (BuyerID != 0 || Individual!=null);

        public override string ToString() => $"Договор №{ID} от {Date?.ToShortDateString()}";
    }
}