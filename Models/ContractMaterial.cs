using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class ContractMaterial : NotifyPropertyChangedBase, ITable
    {
        public int ID { get; set; }
        public int ContactID { get; set; }

        public Material? Material
        {
            get => mat;
            set
            {
                mat = value;
                OnPropertyChanged();
            }
        }

        public float? Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged();
            }
        }

        public ContractMaterial()
        {
            ID = 0;
        }

        public ContractMaterial(int id, int matid, float count)
        {
            ID = id;
            Material = App.DbContext.Materials.ElementAt(matid);
            Count = count;
        }

        public ContractMaterial(int id, Material mat, float count)
        {
            ID = id;
            Material = mat;
            Count = count;
        }

        public bool IsValid => Count != null && Material?.ID != 0;

        private float? count;
        private Material? mat;
    }
}
