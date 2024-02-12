using BuildMaterials.BD;
using DocumentFormat.OpenXml.Office2010.Excel;
using FilterDataGrid.Attributes;

namespace BuildMaterials.Models
{
    public class Account : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        [ColumnName("Дата")]
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }
        [ColumnName("Исполнитель")]
        public Organization? Seller
        {
            get => sel;
            set
            {
                sel = value;
                OnPropertyChanged();
            }
        }
        [ColumnName("Заказчик")]
        public Organization? Buyer
        {
            get => buy;
            set
            {
                buy = value;
                OnPropertyChanged();
            }
        }
        [ColumnName("Договор купли-продажи")]
        public Contract? Contract
        {
            get => con;
            set
            {
                con = value;
                OnPropertyChanged();
            }
        }
        [ColumnName("Сумма")]
        public float Summ
        {
            get
            {
                float value = 0;
                foreach (var item in Contract.Materials)
                {
                    value += (float)item.Material.Price;
                }
                return value+NDS;
            }
        }
        [ColumnName("НДС")]
        public float NDS
        {
            get
            {
                float value = 0;
                foreach(var item in Contract.Materials)
                {
                    value += (float)item.Material.NDS * (float)item.Material.Price;
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

        public Account(int iD, DateTime date, Organization? sel, Organization? buy, Contract contract)
        {
            ID = iD;
            Date = date;
            Seller = sel;
            Buyer = buy;
            Contract = contract;
        }

        public bool IsValid => Date != null
            && Contract?.ID != 0 && Seller?.ID != 0 && Buyer?.ID != 0;
    }
}