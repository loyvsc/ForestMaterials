using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class TN : NotifyPropertyChangedBase, ITable
    {
        #region Public props
        public int ID { get; set; }
        public Contract? Contract
        {
            get => contract;
            set
            {
                contract = value;
                OnPropertyChanged();
            }
        }
        public float Summ
        {
            get
            {
                float summ = 0;
                if (contract != null)
                {
                    foreach (var item in contract.Materials)
                    {
                        if (item.Material != null)
                        {
                            summ += item.Material.Price * item.Material.Count;
                        }
                    }
                }
                return summ;
            }
        }
        public Employee? ResponseEmployee
        {
            get => empl;
            set
            {
                empl = value;
                OnPropertyChanged();
            }
        }
        public Employee? SdalEmployee
        {
            get => sdal;
            set
            {
                sdal = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Private vars
        private Employee? sdal;
        private Employee? empl;
        private Contract? contract;
        #endregion

        #region Constructors
        public TN()
        {
            ID = 0;
        }

        public TN(int iD, Contract contract, Employee responseEmployee, Employee sdalEmployee)
        {
            ID = iD;
            Contract = contract;
            ResponseEmployee = responseEmployee;
            SdalEmployee = sdalEmployee;
        }
        #endregion

        public bool IsValid =>  Contract != null && ResponseEmployee != null && SdalEmployee != null;
    }
}
