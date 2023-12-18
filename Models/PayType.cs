namespace BuildMaterials.Models
{
    public class PayType
    {
        public int ID { get; set; }

        public string Name { get; set; }        

        public PayType()
        {
            ID = 0;
            Name = string.Empty;
        }

        public PayType(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public bool IsValid => Name != string.Empty;
    }
}
