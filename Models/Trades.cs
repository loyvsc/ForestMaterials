using BuildMaterials.BD;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BuildMaterials.Models
{
    public class ViewModelBase : NotifyPropertyChangedBase
    {
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string title;
    }

    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class Trade : NotifyPropertyChangedBase, ITable
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
        public string? DateInString => Date?.ToShortDateString();

        public int? SellerID
        {
            get => sellerId;
            set
            {
                sellerId = value;
                OnPropertyChanged(nameof(SellerID));
                OnPropertyChanged(nameof(Seller));
            }
        }

        public Employee? Seller
        {
            get => seller;
            set
            {
                seller = value;
                SellerID = value.ID;
                OnPropertyChanged(nameof(Seller));
            }
        }

        public int? MaterialID
        {
            get => matId;
            set
            {
                matId = value;
                OnPropertyChanged(nameof(MaterialID));
                OnPropertyChanged(nameof(Material));
            }
        }

        public Material? Material
        {
            get => mat;
            set
            {
                mat = value;
                MaterialID = mat.ID;
                OnPropertyChanged(nameof(Material));
            }
        }

        public float? Count
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

        public int? PayTypeID
        {
            get => pattypeid;
            set
            {
                pattypeid = value;
                OnPropertyChanged(nameof(PayTypeID));
                OnPropertyChanged(nameof(PayType));
            }
        }

        public PayType? PayType
        {
            get => payType;
            set
            {
                if (value != null)
                {
                    payType = value;
                    PayTypeID = value.ID;
                    OnPropertyChanged();
                }
            }
        }

        public float? Summ => Count * Price;

        private PayType? payType;
        private Material? mat;
        private int? sellerId = 0;
        private float? count;
        private float? price;
        private DateTime? date;
        private int? matId = 0;
        private int? pattypeid = 0;
        private Employee? seller;
        public Trade() { }
        public Trade(int iD, DateTime? date, int? sellerid, int? materialId, float? count, float? price, int paytypeid, Employee? empl = null)
        {
            ID = iD;
            Date = date;
            SellerID = sellerid;
            MaterialID = materialId;
            Count = count;
            Price = price;
            PayTypeID = paytypeid;
            Seller = empl;
        }

        public bool IsValid => SellerID != 0
            && MaterialID != 0
            && PayTypeID != 0
            && Count != null
            && Price != null;
    }
}