using BuildMaterials.BD;

namespace BuildMaterials.Models
{

    public class MaterialResponse : NotifyPropertyChangedBase, ITable
    {
        public bool UseBD;
        public int ID { get; set; }

        public int MaterialID
        {
            get => matId;
            set
            {
                matId = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE MaterialResponses SET MaterialID = {value} WHERE ID ={ID};");
                }
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
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE MaterialResponses SET CountUnits = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float BalanceAtStart
        {
            get => balStart;
            set
            {
                balStart = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE MaterialResponses SET BalanceAtStart = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float Prihod
        {
            get => prihod;
            set
            {
                prihod = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE MaterialResponses SET Prihod = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float Rashod
        {
            get => rashod;
            set
            {
                rashod = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE MaterialResponses SET Rashod = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public float BalanceAtEnd
        {
            get => balEnd;
            set
            {
                balEnd = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE MaterialResponses SET BalanceAtEnd = '{value}' WHERE ID ={ID};");
                }
            }
        }
        public int FinResponseEmployeeID
        {
            get => finRespEmpID;
            set
            {
                finRespEmpID = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE MaterialResponses SET FinResponseEmployeeID = '{value}' WHERE ID = {ID};");
                }
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

        public MaterialResponse()
        {
            UseBD = false;
        }
        public MaterialResponse(int iD, int matId, string countUnits, float balStart, float prihod,
            float rashod, float balEnd, int finRespId)
        {
            UseBD = true;
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
            $"Материально-ответственный отчет №{ID}\nМатериально-ответственный сотрудник: {FinReponseEmployee.SurName} {FinReponseEmployee.Name} {FinReponseEmployee.Pathnetic}\nПриход: {prihod}\nРасход: {rashod}\nБаланс на начало: {balStart}\nБаланс на конец: {balEnd}\nНаименование материала: {name}\nЕд. измерения: {countUnits}";

        public bool IsValid => BalanceAtStart >= 0 &&
             Prihod >= 0 && Rashod >= 0 && FinResponseEmployeeID != -1 && MaterialID != -1;
    }
}
