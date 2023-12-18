namespace BuildMaterials.Models
{
    public class Settings
    {
        public string CompanyName { get; set; }
        public string CompanyAdress { get; set; }

        public Settings()
        {
            CompanyName = Properties.Settings.Default.CompanyName;
            CompanyAdress = Properties.Settings.Default.CompanyAdress;
        }

        public void Save()
        {
            Properties.Settings.Default.CompanyName = CompanyName;
            Properties.Settings.Default.Save();
        }

        public bool IsValid => CompanyName != null && CompanyAdress!=null;
    }
}