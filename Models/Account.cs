using BuildMaterials.BD;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace BuildMaterials.Models
{
    public class Account : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }
        public Organization? Seller
        {
            get => sel;
            set
            {
                sel = value;
                OnPropertyChanged();
            }
        }
        public Organization? Buyer
        {
            get => buy;
            set
            {
                buy = value;
                OnPropertyChanged();
            }
        }
        public Contract? Contract
        {
            get => con;
            set
            {
                con = value;
                OnPropertyChanged();
            }
        }
        public float Summ
        {
            get
            {
                float value = 0;
                foreach (var item in Contract.Materials)
                {
                    value += item.Material.Price;
                }
                return value+NDS;
            }
        }
        public float NDS
        {
            get
            {
                float value = 0;
                foreach(var item in Contract.Materials)
                {
                    value += item.Material.NDS * item.Material.Price;
                }
                return value;
            }
        }

        private Contract? con;
        private Organization? buy;
        private Organization? sel;
        private DateTime? date;

        public Account()
        {
            ID = 0;            
            Seller = new Organization();
            Buyer = new Organization();
            Contract = new Contract();
        }

        public Account(int iD, DateTime date, Organization? sel, Organization? buy)
        {
            ID = iD;
            Date = date;
            Seller = sel;
            Buyer = buy;
        }

        public bool IsValid => Date != null
            && Contract?.ID != 0 && Seller?.ID != 0 && Buyer?.ID != 0;
    }
}