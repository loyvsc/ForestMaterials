using BuildMaterials.BD;
using BuildMaterials.Export;

namespace BuildMaterials.Models
{

    public class MaterialResponse : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }

        [IgnoreProperty]
        public int MaterialID
        {
            get => matId;
            set
            {
                matId = value;
                OnPropertyChanged();
            }
        }
        private int matId;
        public Material? Material => App.DbContext.Materials.ElementAt(MaterialID);
        public string CountUnits
        {
            get => countUnits;
            set
            {
                countUnits = value;
                OnPropertyChanged();
            }
        }
        public float BalanceAtStart
        {
            get => balStart;
            set
            {
                balStart = value;
                OnPropertyChanged();
            }
        }
        public float Prihod
        {
            get => prihod;
            set
            {
                prihod = value;
                OnPropertyChanged();
            }
        }
        public float Rashod
        {
            get => rashod;
            set
            {
                rashod = value;
                OnPropertyChanged();
            }
        }
        public float BalanceAtEnd
        {
            get => balEnd;
            set
            {
                balEnd = value;
                OnPropertyChanged();
            }
        }
        public int FinResponseEmployeeID
        {
            get => finRespEmpID;
            set
            {
                finRespEmpID = value;
                OnPropertyChanged(nameof(FinResponseEmployeeID));
                OnPropertyChanged(nameof(FinReponseEmployee));
            }
        }
        public Employee? FinReponseEmployee
        {
            get => App.DbContext.Employees.ElementAt(FinResponseEmployeeID);
            set
            {
                if (value != null)
                {
                    FinResponseEmployeeID = value.ID;
                    OnPropertyChanged(nameof(FinReponseEmployee));
                }
            }
        }

        private int finRespEmpID = -1;
        private float balEnd = 0;
        private float rashod = 0;
        private float prihod = 0;
        private float balStart = 0;
        private string countUnits = string.Empty;
        private string name = string.Empty;

        public MaterialResponse() { }

        public MaterialResponse(int iD, int matId, string countUnits, float balStart, float prihod,
            float rashod, float balEnd, int finRespId)
        {
            ID = iD;
            CountUnits = countUnits;
            BalanceAtStart = balStart;
            BalanceAtEnd = balEnd;
            Prihod = prihod;
            Rashod = rashod;
            BalanceAtEnd = balEnd;
            FinResponseEmployeeID = finRespId;
            MaterialID = matId;
        }

        public override string ToString() =>
            $"Материально-ответственный отчет №{ID}\nМатериально-ответственный сотрудник: {FinReponseEmployee.Surname} {FinReponseEmployee.Name} {FinReponseEmployee.Pathnetic}\nПриход: {prihod}\nРасход: {rashod}\nБаланс на начало: {balStart}\nБаланс на конец: {balEnd}\nНаименование материала: {name}\nЕд. измерения: {countUnits}";

        public bool IsValid => BalanceAtStart >= 0 &&
             Prihod >= 0 && Rashod >= 0 && FinResponseEmployeeID != -1 && MaterialID != -1;
    }
}
