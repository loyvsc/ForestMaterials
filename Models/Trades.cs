using BuildMaterials.BD;
using System.ComponentModel;

namespace BuildMaterials.Models
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class Trade : NotifyPropertyChangedBase, ITable
    {
        private readonly bool UseBD;

        public int ID { get; set; }
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE trades SET Date='{date.ToMySQLDate()}' WHERE ID = {ID};");
                }
            }
        }
        public string? DateInString
        {
            get => Date.ToShortDateString();
            set => Date = Convert.ToDateTime(value);
        }

        public int? SellerID
        {
            get => sellerId;
            set
            {
                sellerId = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE trades SET SellerID={value} WHERE ID = {ID};");
                    OnPropertyChanged(nameof(SellerID));
                    OnPropertyChanged(nameof(Seller));
                }
            }
        }

        public Employee? Seller
        {
            get => SellerID != null ? App.DbContext.Employees.Select($"SELECT * FROM employees WHERE ID = {SellerID};")[0] : null;
            set
            {
                if (value != null)
                {
                    SellerID = value.ID;
                    OnPropertyChanged(nameof(Seller));
                }
            }
        }
        public int? MaterialID
        {
            get => matId;
            set
            {
                matId = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE trades SET MaterialID={value} WHERE ID = {ID};");
                    OnPropertyChanged(nameof(MaterialID));
                    OnPropertyChanged(nameof(Seller));
                }
            }
        }

        public Material? Material
            => MaterialID != null ? App.DbContext.Materials.Select($"SELECT * FROM Materials WHERE ID={MaterialID}")[0] : null;

        public float? Count
        {
            get => count;
            set
            {
                count = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE trades SET Count={value} WHERE ID = {ID};");
                }
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
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE trades SET Price={value} WHERE ID = {ID};");
                }
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
            }
        }

        public int? PayTypeID
        {
            get => pattypeid;
            set
            {
                pattypeid = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE trades SET PayTypeID={value} WHERE ID = {ID};");
                }
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Summ));
            }
        }

        public PayType? PayType
        {
            get => App.DbContext.PayTypes.Select($"SELECT * FROM PAYTYPES WHERE ID = {PayTypeID}")[0];
            set
            {
                if (PayType != null)
                {
                    PayTypeID = value!.ID;
                    OnPropertyChanged(nameof(PayType));
                }
            }
        }

        public float? Summ => Count * Price;

        private int? sellerId = -1;
        private float? count = 0;
        private float? price = 0;
        private DateTime date;
        private int? matId = -1;
        private int? pattypeid;

        public Trade()
        {
            UseBD = false;
            Date = DateTime.Now;
        }

        public Trade(int iD, DateTime date, int? sellerid, int? materialId, float? count, float? price, int paytypeid)
        {
            UseBD = false;
            ID = iD;
            Date = date;
            SellerID = sellerid;
            MaterialID = materialId;
            Count = count;
            Price = price;
            PayTypeID = paytypeid;
            UseBD = true;
        }

        public bool IsValid => sellerId != -1 && matId != -1;

        public override string ToString() => $"Товарооборот №{ID} от {DateInString}\nПродавец: {Seller}\nМатериал: {Material?.Name}\nКоличество: {Count}\nЦена: {Price}";
    }
}