using BuildMaterials.BD;

namespace BuildMaterials.Models
{
    public class Organization : ITable
    {
        public bool UseBD;
        public int ID { get; set; }
        public string? UNP
        {
            get => unp;
            set
            {
                unp = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Sellers SET UNP ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? CompanyName
        {
            get => companyName;
            set
            {
                companyName = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Sellers SET CompanyName ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? ShortCompamyName
        {
            get => shtrcmpname;
            set
            {
                shtrcmpname = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Sellers SET ShortCompamyName ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? Adress
        {
            get => adress;
            set
            {
                adress = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Sellers SET Adress ='{value}' WHERE ID={ID};");
                }
            }
        }
        public DateTime? RegistrationDate
        {
            get => regdat;
            set
            {
                regdat = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Sellers SET RegistrationDate ='{value.Value.ToMySQLDate()}' WHERE ID={ID};");
                }
            }
        }
        public string? RegistrationDateInString => RegistrationDate?.ToString("d");
        public string? MNSNumber
        {
            get => msnnum;
            set
            {
                msnnum = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Sellers SET mnsnumber ='{value}' WHERE ID={ID};");
                }
            }
        }
        public string? MNSName
        {
            get => msnname;
            set
            {
                msnname = value;
                if (UseBD)
                {
                    App.DbContext.Query($"UPDATE Sellers SET mnsname ='{value}' WHERE ID={ID};");
                }
            }
        }

        private string? msnname;
        private string? msnnum;
        private string? shtrcmpname;
        private string? companyName;
        private string? adress;
        private string? unp;
        private DateTime? regdat;

        public Organization()
        {
            UNP = "";
            UseBD = false;
        }

        public Organization(int iD, string? companyName, string? shortCompanyName, string? adress, DateTime? regDate, string? mnsNum, string? mnsName, string? uNP)
        {
            UseBD = false;
            ID = iD;
            CompanyName = companyName;
            Adress = adress;
            ShortCompamyName = shortCompanyName;
            RegistrationDate = regDate;
            MNSName = mnsName;
            MNSNumber = mnsNum;
            UNP = uNP;
            UseBD = true;
        }
    }
}