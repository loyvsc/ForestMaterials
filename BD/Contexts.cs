using BuildMaterials.Extensions;
using BuildMaterials.Helpers;
using BuildMaterials.Models;
using BuildMaterials.ViewModels;
using System.Collections.ObjectModel;

namespace BuildMaterials.BD
{
    internal static class StaticValues
    {
        public const string DatabaseName = "leshoz";
        public const string ConnectionString = $"server=localhost;user=root;database={DatabaseName};password=546909023Var;";
        public const string CreateDatabaseConnectionString = "server=localhost;user=root;password=546909023Var;";
    }
    internal interface IDBSetBase<T> where T : class
    {
        void Add(T obj);
        void Remove(T obj);
        void Remove(int id);
        List<T> Select(string query);
        void AddRange(IEnumerable<T> entities)
        {
            foreach(var item in entities)
            {
                Add(item);
            }
        }
        void Update(T obj);
    }

    public class ApplicationContext
    {
        #region Tables
        public ContactTable Contacts { get; }
        public MaterialsTable Materials { get; }
        public EmployeesTable Employees { get; }
        public OrganizationsTable Organizations { get; }
        public TradesTable Trades { get; }
        public TTNSTable TTNs { get; }
        public AccountsTable Accounts { get; }
        public ContractsTable Contracts { get; }
        public MaterialResponsesTable MaterialResponse { get; }
        public PayTypesTable PayTypes { get; }
        public PassportsTable Passports { get; }
        public IndividualsTable Individuals { get; }
        public ContractMaterialsTable ContractMaterials { get; }
        public TNTable TNs { get; }
        public AutomobilesTable Automobiles { get; }
        #endregion

        private bool CheckBDCreated()
        {
            bool res = false;
            using (MySqlConnection con = new MySqlConnection(StaticValues.CreateDatabaseConnectionString))
            {
                con.Open();
                MySqlCommand comm = new MySqlCommand($"SELECT * FROM information_schema.tables WHERE table_schema = '{StaticValues.DatabaseName}' LIMIT 1", con);
                MySqlDataReader reader = comm.ExecuteReader();
                res = reader.HasRows;
                con.Close();
            }
            return res;
        }

        public ApplicationContext()
        {
            Contacts = new ContactTable();
            PayTypes = new PayTypesTable();
            Employees = new EmployeesTable();
            Organizations = new OrganizationsTable();
            Materials = new MaterialsTable();
            Trades = new TradesTable();
            TTNs = new TTNSTable();
            Accounts = new AccountsTable();
            Contracts = new ContractsTable();
            MaterialResponse = new MaterialResponsesTable();
            Individuals = new IndividualsTable();
            Passports = new PassportsTable();
            ContractMaterials = new ContractMaterialsTable();
            TNs = new TNTable();
            Automobiles = new AutomobilesTable();
            CreateDatabase();
        }

        public void CreateDatabase()
        {
            if (CheckBDCreated() == false)
            {
                InitializeDatabase();
                Employees.Add(new Employee(-1, "Имя", "Фамилия", "Отчество", "Администратор", "+375259991234", new Passport(0, "BM1234567", new DateTime(2016, 3, 12), "РУВД ПОЛОЦК"), "", false, true, true, true, true));                                
                
                PayTypes.Add(new PayType(-1, "Наличный расчет"));
                PayTypes.Add(new PayType(-1, "Безналичный расчет"));
            }
        }

        private void InitializeDatabase()
        {
            InitDatabase();
            InitTables();
        }

        private void InitTables()
        {
            Query(EmployeesTable.CreateQuery + OrganizationsTable.CreateQuery + MaterialsTable.CreateQuery +
                TradesTable.CreateQuery + TTNSTable.CreateQuery + AccountsTable.CreateQuery + ContractsTable.CreateQuery
                + MaterialResponsesTable.CreateQuery + PayTypesTable.CreateQuery + ContactTable.CreateQuery + IndividualsTable.CreateQuery
                + PassportsTable.CreateQuery + ContractMaterialsTable.CreateQuery + TNTable.CreateQuery + AutomobilesTable.CreateQuery);
            CreateFKs();
        }

        private void CreateFKs()
        {
            string FKsQuery = "ALTER TABLE TRADES ADD FOREIGN KEY (MaterialID) REFERENCES MATERIALS(ID) ON DELETE cascade;" +
                "ALTER TABLE TRADES ADD FOREIGN KEY (SellerId) REFERENCES EMPLOYEES(ID) ON DELETE cascade;" +
                "ALTER TABLE TRADES ADD FOREIGN KEY (paytypeid) REFERENCES paytypes(ID) ON DELETE SET NULL;" +
                "ALTER TABLE materialresponses add foreign key (MaterialID) references materials(id) on delete set null;" +
                "ALTER TABLE materialresponses add foreign key (FinResponseEmployeeID) references EMPLOYEES(id) on delete set null;" +
                "ALTER TABLE ttns add foreign key (automobileid) references automobiles(id) on delete cascade;" +
                "ALTER TABLE ttns add foreign key (contractid) references contracts(id) on delete cascade;" +
                "ALTER TABLE ttns add foreign key (sdalEmployee) references EMPLOYEES(id) on delete cascade;" +
                "ALTER TABLE ttns add foreign key (responseEmployee) references EMPLOYEES(id) on delete cascade;" +
                "ALTER TABLE accounts add foreign key (contractid) references contracts(id) on delete cascade;" +
                "ALTER TABLE accounts add foreign key (sellerid) references sellers(id) on delete cascade;" +
                "ALTER TABLE accounts add foreign key (Buyerid) references sellers(id) on delete cascade;" +
                "ALTER TABLE contracts add foreign key (Buyer) references sellers(id) on delete set null;" +
                "ALTER TABLE contracts add foreign key (seller) references sellers(id) on delete set null;" +
                "ALTER TABLE individuals add foreign key (passportid) references passports(id) on delete cascade;" +
                "ALTER TABLE employees add foreign key (passportid) references passports(id) on delete cascade;" +
                "ALTER TABLE contractmaterials add foreign key (contractid) references contracts(id) on delete cascade;" +
                "ALTER TABLE contractmaterials add foreign key (materialid) references MATERIALS(id) on delete cascade;" +
                "ALTER TABLE tns add foreign key (contractid) references contracts(id) on delete cascade;" +
                "ALTER TABLE tns add foreign key (responseemployeeid) references employees(id) on delete cascade;" +
                "ALTER TABLE tns add foreign key (sdalemployeeid) references employees(id) on delete cascade;";
            Query(FKsQuery);
        }

        private static void InitDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(StaticValues.CreateDatabaseConnectionString))
            {
                connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS " + StaticValues.DatabaseName, connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                connection.CloseAsync().Wait();
            }
        }

        public void Query(string query)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }
    }

    public class ContactTable : IDBSetBase<Contact>
    {
        private readonly MySqlConnection _connection;
        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS contacts " +
                "(ID int NOT NULL AUTO_INCREMENT, organizationid int not null, contacttypeid int not null, text varchar(200) null," +
                "PRIMARY KEY (ID));";        

        public ContactTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        private static int getContactTypeIndex(ContactType type)
        {
            switch (type)
            {
                case ContactType.Email: { return 1; }
                case ContactType.Phonenumber: { return 2; }
                default: { return 0; }
            }
        }
        private static ContactType getContactTypeByIndex(int index)
        {
            switch (index)
            {
                case 1: { return ContactType.Email; }
                case 2: { return ContactType.Phonenumber; }
                default: { return 0; }
            }
        }
        public void Add(Contact obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("INSERT INTO contacts " +
                "(organizationid, contacttypeid, text) VALUES" +
                $"({obj.OrganizationID},{getContactTypeIndex(obj.ContactType)},'{obj.Text}');", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(Contact obj) => Remove(obj.ID);

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM Contacts WHERE ID = {id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Contact> Select(string query)
        {
            List<Contact> materials = new List<Contact>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetContact(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }

        public List<Contact> ToList() => Select("SELECT * FROM CONTACTS");

        private Contact GetContact(MySqlDataReader reader)
        {
            int id = (int)reader[0];
            int orgid = (int)reader[1];
            ContactType type = getContactTypeByIndex((int)reader[2]);
            string text = (string)reader[3];

            return new Contact(id, orgid, type, text);
        }

        public void Update(Contact obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE contacts SET organizationid = {obj.OrganizationID}," +
                $"contacttypeid = {getContactTypeIndex(obj.ContactType)}, text = '{obj.Text}' WHERE ID = {obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
    }
    public class MaterialResponsesTable : IDBSetBase<MaterialResponse>
    {
        private readonly MySqlConnection _connection;
        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS materialresponses " +
                "(ID int NOT NULL AUTO_INCREMENT, MaterialID int null, CountUnits varchar(100), BalanceAtStart float not null, " +
                "Prihod float not null, Rashod float not null, BalanceAtEnd float not null," +
                "FinResponseEmployeeID int null, PRIMARY KEY (ID));";

        public MaterialResponsesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public MaterialResponse ElementAt(int id)
        {
            MaterialResponse material = new MaterialResponse();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM materialresponses WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        material.ID = reader.GetInt32(0);
                        material.MaterialID = reader.GetInt32(1);
                        material.BalanceAtStart = reader.GetFloat(2);
                        material.BalanceAtStart = reader.GetFloat(3);
                        material.Prihod = reader.GetFloat(4);
                        material.Rashod = reader.GetFloat(5);
                        material.BalanceAtEnd = reader.GetFloat(6);
                        material.FinResponseEmployeeID = reader.GetInt32(7);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return material;
        }

        public void Update(MaterialResponse obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE materialresponses SET MaterialID = '{obj.MaterialID}', CountUnits = '{obj.CountUnits}'," +
                $"BalanceAtStart = {obj.BalanceAtStart}, Prihod = {obj.Prihod}," +
                $"Rashod = {obj.Rashod}, FinResponseEmployeeID = {obj.FinResponseEmployeeID} WHERE ID = {obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Add(MaterialResponse obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("INSERT INTO materialresponses " +
                "(MaterialID, CountUnits, BalanceAtStart, Prihod, Rashod, BalanceAtEnd, FinResponseEmployeeID) VALUES" +
                $"({obj.MaterialID},'{obj.CountUnits}',{obj.BalanceAtStart},{obj.Prihod}," +
                $"{obj.Rashod},{obj.BalanceAtEnd},{obj.FinResponseEmployeeID});", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(MaterialResponse obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materialresponses WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<MaterialResponse> Search(string text)
        {
            return Select($"SELECT * FROM materialresponses WHERE Name like '%{text}%';");
        }

        public List<MaterialResponse> Select(string query)
        {
            List<MaterialResponse> materials = new List<MaterialResponse>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetMaterialResponse(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }

        private MaterialResponse GetMaterialResponse(MySqlDataReader reader)
        {
            MaterialResponse material = new MaterialResponse(reader.GetInt32(0), reader.GetInt32(1),
                reader.GetString(2), reader.GetFloat(3), reader.GetFloat(4), reader.GetFloat(5),
                reader.GetFloat(6), reader.GetInt32(7));
            return material;
        }

        public List<MaterialResponse> ToList()
        {
            return Select("SELECT * FROM MATERIALRESPONSES;");
        }
    }
    public class TNTable : IDBSetBase<TN>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS tns (ID int NOT NULL AUTO_INCREMENT, contractId int not null, ResponseEmployeeID int not null, SdalEmployeeID int not null, PRIMARY KEY (ID));";

        public TNTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        private TN GetTN(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            var contract = App.DbContext.Contracts.ElementAt(reader.GetInt32(1));
            var resp = App.DbContext.Employees.ElementAt(reader.GetInt32(2));
            var sdal = App.DbContext.Employees.ElementAt(reader.GetInt32(3));
            return new TN(id, contract, resp, sdal);
        }
        public TN ElementAt(int id)
        {
            TN material = new TN();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM tns WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        return GetTN(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return material;
        }
        public void Add(TN obj)
        {
            string query = "INSERT INTO tns (contractId,ResponseEmployeeID,SdalEmployeeID) VALUES (@c,@r,@s);";

            var connection = new MySqlConnection(StaticValues.ConnectionString);
            connection.Open();
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@c", obj.Contract!.ID);
            cmd.Parameters.AddWithValue("@r", obj.ResponseEmployee!.ID);
            cmd.Parameters.AddWithValue("@s", obj.SdalEmployee!.ID);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void Update(TN obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("UPDATE tns SET contractId = @c, ResponseEmployeeID = @r, SdalEmployeeID = @s WHERE ID = @i", _connection))
            {
                command.Parameters.AddWithValue("@c", obj.Contract!.ID);
                command.Parameters.AddWithValue("@r", obj.ResponseEmployee!.ID);
                command.Parameters.AddWithValue("@s", obj.SdalEmployee!.ID);
                command.Parameters.AddWithValue("@i", obj.ID);
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
        public void Remove(TN obj) => Remove(obj.ID);
        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("DELETE FROM tns WHERE id=@i;", _connection))
            {
                command.Parameters.AddWithValue("@i", id);
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
        public List<TN> Select(string query)
        {
            List<TN> materials = new List<TN>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetTN(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }
        public List<TN> ToList() => Select("SELECT * FROM tns;");
    }
    public class MaterialsTable : IDBSetBase<Material>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS materials" +
                "(ID int NOT NULL AUTO_INCREMENT, Name varchar(300) not null," +
                "Price float NOT NULL," +
                "Count float NOT NULL,CountUnits varchar(20)," +
                "EnterDate datetime NOT NULL, shirina varchar(50) null, dlina varchar(50) null, nds float not null, sort varchar(300) null, PRIMARY KEY (ID));";

        public MaterialsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public Material ElementAt(int id)
        {
            Material material = new Material();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Materials WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        return GetMaterial(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return material;
        }

        public void Add(Material obj)
        {
            string query = "INSERT INTO materials (Name, Price, Count, CountUnits, EnterDate, dlina, shirina, nds, sort) VALUES (@n, @p, @c, @cu, @ed, @d, @sh, @nds, @s);";

            var connection = new MySqlConnection(StaticValues.ConnectionString);
            connection.Open();
            var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@n", obj.Name);
            cmd.Parameters.AddWithValue("@p", obj.Price);
            cmd.Parameters.AddWithValue("@c", obj.Count);
            cmd.Parameters.AddWithValue("@cu", obj.CountUnits);
            cmd.Parameters.AddWithValue("@ed", obj.EnterDate);
            cmd.Parameters.AddWithValue("@d", obj.Dlina);
            cmd.Parameters.AddWithValue("@sh", obj.Shirina);
            cmd.Parameters.AddWithValue("@nds", obj.NDS);
            cmd.Parameters.AddWithValue("@s", obj.Sort);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void Update(Material obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE materials SET Name = '{obj.Name}'," +
                $"Price = @p, Count = @c, CountUnits = '{obj.CountUnits}', nds = @nds EnterDate = '{obj.EnterDate.ToMySQLDate()}', shirina = '{obj.Shirina}', dlina = '{obj.Dlina}', sort = '{obj.Sort}' WHERE ID = {obj.ID};", _connection))
            {
                command.Parameters.AddWithValue("@p", obj.Price);
                command.Parameters.AddWithValue("@nds", obj.NDS);
                command.Parameters.AddWithValue("@c", obj.Count);
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(Material obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Material> Search(string text)
        {
            return Select($"SELECT * FROM Materials WHERE CONCAT(name,' ',' ',enterdate) like '%{text}%';");
        }

        public List<Material> Select(string query)
        {
            List<Material> materials = new List<Material>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        materials.Add(GetMaterial(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return materials;
        }

        private Material GetMaterial(MySqlDataReader reader)
        {
            var shir = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
            var dlin = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
            return new Material(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), reader.GetFloat(3), reader.GetString(4), reader.GetDateTime(5), shir, dlin, reader.GetString(9), reader.GetFloat(8));
        }

        public List<Material> ToList() => Select("SELECT * FROM Materials;");
    }
    public class EmployeesTable : IDBSetBase<Employee>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS employees " +
                "(ID int NOT NULL AUTO_INCREMENT, Name varchar(300), Surname varchar(300)," +
                "Pathnetic varchar(300), Position varchar(300), PhoneNumber varchar(300)," +
                "Password varchar(300), FinResponsible boolean, passportid int, canadd boolean, canedit boolean, candel boolean, isadmin boolean, PRIMARY KEY (ID));";

        public EmployeesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM employees;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Employee GetEmployee(MySqlDataReader reader)
        {
            int id = (int)reader[0];
            string name = ((string)reader[1]).DecryptText();
            string surname = ((string)reader[2]).DecryptText();
            string pathnetic = ((string)reader[3]).DecryptText();
            string position = (string)reader[4];
            string phonenumber = ((string)reader[5]).DecryptText();
            string password = (string)reader[6];
            bool finresp = (bool)reader[7];
            int pasid = (int)reader[8];
            bool canadd = (bool)reader[9];
            bool canedit = (bool)reader[10];
            bool candel = (bool)reader[11];
            bool isadmin = (bool)reader[12];

            Passport passport = App.DbContext.Passports.ElementAt(pasid);
            return new Employee(id, name, surname, pathnetic, position, phonenumber, passport, password, finresp,
                canadd, canedit, candel, isadmin);
        }

        public Employee ElementAt(int id)
        {
            Employee employee = null!;
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Employees WHERE id={id};", _connection))
            {
                using (MySqlDataReader reader = command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        employee = GetEmployee(reader);
                    }
            }
            _connection.CloseAsync().Wait();
            return employee;
        }

        public void Update(Employee obj)
        {
            App.DbContext.Passports.Update(obj.Passport);
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE employees SET Name = '{obj.Name.EncryptText()}', Surname = '{obj.Surname.EncryptText()}'," +
                $"pathnetic = '{obj.Pathnetic.EncryptText()}', position = '{obj.Position}', phonenumber = '{obj.PhoneNumber.EncryptText()}'," +
                $"password = '{obj.Password.EncryptText()}', FinResponsible = {obj.FinResponsible}, canadd = {obj.CanUserAdd}, canedit = {obj.CanUserEdit}," +
                $"candel = {obj.CanUserDelete}, isadmin = {obj.IsUserAdmin} WHERE ID = {obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Add(Employee obj)
        {
            
            using (MySqlCommand command = new MySqlCommand(
                $"INSERT INTO passports(number, issuedate, issuepunkt) VALUES('{obj.Passport.Number.EncryptText()}', '{obj.Passport.IssueDate?.ToMySQLDate().EncryptText()}','{obj.Passport.IssuePunkt.EncryptText()}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
            int id = 0;
            using (MySqlCommand command = new MySqlCommand(
                $"SELECT ID FROM PASSPORTS WHERE NUMBER = '{obj.Passport.Number.EncryptText()}'", _connection))
            {
                _connection.OpenAsync().Wait();
                var reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    id = (int)reader[0];
                }
                _connection.CloseAsync().Wait();
            }

            using (MySqlCommand command = new MySqlCommand(
                "INSERT INTO employees " +
                $"(Name, Surname, pathnetic, position, phonenumber, password, FinResponsible,passportid, canadd, canedit, candel, isadmin) VALUES" +
                $"('{obj.Name.EncryptText()}','{obj.Surname.EncryptText()}'," +
                $"'{obj.Pathnetic.EncryptText()}','{obj.Position}','{obj.PhoneNumber.EncryptText()}','{obj.Password}',{(obj.FinResponsible ? 1 : 0)}," +
                $"{id},{obj.CanUserAdd},{obj.CanUserEdit},{obj.CanUserDelete},{obj.IsUserAdmin});", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Employee obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM employees WHERE id={obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM employees WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Employee> Search(string text)
        {
            List<Employee> employees = new List<Employee>(64);
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Employees WHERE " +
                $"CONCAT(name,' ', surname,' ', pathnetic,' ',position,' ',phonenumber) like '%{text}%';", _connection))
            {
                MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    employees.Add(GetEmployee(reader));
                }
            }
            _connection.CloseAsync().Wait();
            return employees;
        }

        public List<Employee> ToList() => Select("SELECT * FROM employees;");

        public List<Employee> Select(string query)
        {
            List<Employee> employees = new List<Employee>(64);
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    employees.Add(GetEmployee(reader));
                }
            }
            _connection.CloseAsync().Wait();
            return employees;
        }
    }
    public class OrganizationsTable
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS sellers " +
                "(ID int NOT NULL AUTO_INCREMENT, CompanyName varchar(200), Adress varchar(200)," +
                "ShortCompanyName varchar(70), RegistrationDate date, mnsnumber varchar(5)," +
                "mnsname varchar(100) NOT NULL, UNP varchar(100) NOT NULL, rasch varchar(200), cbu varchar(100), bik varchar(10) null, curScht varchar(28) null, PRIMARY KEY (ID));";

        public OrganizationsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM Sellers;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Organization GetSeller(MySqlDataReader reader)
        {
            int id = (int)reader["id"];
            string compName = reader.IsDBNull(1) ? string.Empty : (string)reader[1];
            string address = reader.IsDBNull(2) ? string.Empty : (string)reader[2];
            string shortcmpname = reader.IsDBNull(3) ? string.Empty : (string)reader[3];
            DateTime? regdate = reader.IsDBNull(4) ? null : (DateTime)reader[4];
            string mnsnum = reader.IsDBNull(5) ? string.Empty : (string)reader[5];
            string mnsname = reader.IsDBNull(6) ? string.Empty : (string)reader[6];
            string unp = reader.IsDBNull(7) ? string.Empty : (string)reader[7];
            string rasch = reader.IsDBNull(8) ? string.Empty : (string)reader[8];
            string cbu = reader.IsDBNull(9) ? string.Empty : (string)reader[9];
            string bik = reader.IsDBNull(10) ? string.Empty : (string)reader[10];
            string curScht = reader.IsDBNull(11) ? string.Empty : (string)reader[11];

            ObservableCollection<Contact> contacts = new ObservableCollection<Contact>(App.DbContext.Contacts.Select("SELECT * FROM CONTACTS WHERE ORGANIZATIONID = " + id));

            return new Organization(id, compName, shortcmpname, address, regdate, mnsnum, mnsname, unp, rasch, cbu, bik, curScht, contacts);
        }

        public Models.Organization ElementAt(int id)
        {
            Organization obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM Sellers WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetSeller(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }

        public void Update(Organization obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE Sellers SET companyname = '{obj.CompanyName}', adress = '{obj.Adress}'," +
                $"ShortCompanyName = '{obj.ShortCompamyName}', RegistrationDate = '{obj.RegistrationDate?.ToMySQLDate()}'," +
                $"mnsnumber = '{obj.MNSNumber}', mnsname = '{obj.MNSName}', UNP = '{obj.UNP}', rasch = '{obj.RascSchet}', cbu = '{obj.CBU}'," +
                $"bik = '{obj.BIK}', curScht ='{obj.CurrentSchet}' WHERE ID = {obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Add(Organization obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO Sellers " +
                "(companyname, adress, ShortCompanyName, RegistrationDate, mnsnumber, mnsname, UNP, rasch, cbu, bik, curScht) VALUES" +
                $"('{obj.CompanyName}','{obj.Adress}'," +
                $"'{obj.ShortCompamyName}','{obj.RegistrationDate!.Value.ToMySQLDate()}','{obj.MNSNumber}','{obj.MNSName}','{obj.UNP}','{obj.RascSchet}','{obj.CBU}'," +
                $"'{obj.BIK}', '{obj.CurrentSchet}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Organization obj) => Remove(obj.ID);

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM Sellers WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Organization> Search(string text) =>
            Select($"SELECT * FROM Sellers WHERE CONCAT(companyname,' ', adress,' ', companyperson,' ',bank,' ',bankprop,' ', unp,' ',rasch,' ', cbu, ' ', bik, ' ', curScht) like '%{text}%';");

        public List<Organization> ToList() => Select($"SELECT * FROM sellers;");

        public List<Organization> Select(string query)
        {
            List<Organization> providers = new List<Organization>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        providers.Add(GetSeller(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return providers;
        }
    }
    public class TradesTable : IDBSetBase<Trade>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS trades " +
                "(ID int NOT NULL AUTO_INCREMENT, Date date not null, SellerId int NULL, " +
                "MaterialID int not null, count float, price float,paytypeid int, PRIMARY KEY (ID));";

        public TradesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM trades;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Trade GetTrade(MySqlDataReader reader)
        {
            int id = (int)reader["id"];
            DateTime date = (DateTime)reader["Date"];
            int? sellerid = reader.IsDBNull(2) ? null : (int)reader["sellerid"];
            Employee empl = App.DbContext.Employees.Select($"SELECT * FROM employees WHERE ID = {sellerid};")[0];
            int? materialid = reader.IsDBNull(3) ? null : (int)reader["materialid"];
            Material mat = App.DbContext.Materials.ElementAt((int)(materialid!));
            float? count = reader.IsDBNull(4) ? null : (float)reader["count"];
            float? price = reader.IsDBNull(5) ? null : (float)reader["price"];
            int paytypeid = (int)reader[6];
            PayType? paytype = App.DbContext.PayTypes.Select($"SELECT * FROM PAYTYPES WHERE ID = {paytypeid}")[0];
            return new Trade(id, date, sellerid, materialid, count, price, paytypeid, empl) { Material = mat, PayType = paytype };
        }


        public Trade ElementAt(int id)
        {
            Trade obj = null!;
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"SELECT * FROM trades WHERE id={id};", _connection))
            {
                MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    obj = GetTrade(reader);
                }
            }
            _connection.CloseAsync().Wait();
            return obj;
        }

        public void Add(Trade obj)
        {
            string sellerParam = obj.SellerID != null ? "SellerID," : "";
            string sellerValue = obj.SellerID != null ? obj.SellerID + "," : "";
            using (MySqlCommand command = new MySqlCommand("INSERT INTO trades " +
                $"(Date, {sellerParam} MaterialID, Count, Price,paytypeid) VALUES" +
                $"('{obj.Date?.ToMySQLDate()}',{sellerValue}" +
                $"{obj.MaterialID},{obj.Count},{obj.Price},{obj.PayTypeID});", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
            App.DbContext?.Query($"UPDATE Materials SET COUNT = COUNT-{obj.Count} WHERE id = {obj.Material.ID};");
        }

        public void Remove(Trade obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM trades WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Update(Trade obj) => App.DbContext.Query($"UPDATE Trades SET Count = {obj.Count}, Date = '{obj.Date?.ToMySQLDate()}'," +
            $"MaterialID = {obj.MaterialID},PayTypeID = {obj.PayTypeID},Price = {obj.Price},SellerID = {obj.SellerID} " +
            $"WHERE ID = {obj.ID};");

        public List<Trade> Search(string text)
        {
            return App.DbContext.Trades.Select($"SELECT * FROM trades WHERE CONCAT(SellerFio,' ', MaterialName) like '%{text}%';)");
        }

        public List<Trade> ToList() => Select("SELECT * FROM trades;");

        public List<Trade> Select(string query)
        {
            List<Trade> trades = new List<Trade>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        trades.Add(GetTrade(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return trades;
        }
    }
    public class AutomobilesTable : IDBSetBase<Automobile>
    {
        private readonly MySqlConnection _connection;
        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS automobiles " +
                "(ID int NOT NULL AUTO_INCREMENT, " +
                "name varchar(200) not null, number varchar(9) not null, PRIMARY KEY (ID));";

        public AutomobilesTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }
        private Automobile GetAutomobile(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string? name = reader.IsDBNull(1) ? null : (string)reader[1];
            string? number = reader.IsDBNull(2) ? null : (string)reader[2];
            return new Automobile(id, name, number);
        }
        public Automobile ElementAt(int id)
        {
            Automobile obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM automobiles WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetAutomobile(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }
        public void Update(Automobile obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE automobiles SET name = @n, number = @num WHERE ID = {obj.ID};", _connection))
            {
                command.Parameters.AddWithValue("@n", obj.Name);
                command.Parameters.AddWithValue("@num", obj.RegistrationNumber);
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
        public void Add(Automobile obj)
        {
            var connection = new MySqlConnection(StaticValues.ConnectionString);
            using (MySqlCommand command = new MySqlCommand("INSERT INTO automobiles (name,number) VALUES (@n, @reg);", connection))
            {
                command.Parameters.AddWithValue("@n", obj.Name);
                command.Parameters.AddWithValue("@reg", obj.RegistrationNumber);
                connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                connection.CloseAsync().Wait();
            }
        }
        public void Remove(Automobile obj) => Remove(obj.ID);
        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("DELETE FROM automobiles WHERE id=" + id, _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
        public List<Automobile> Select(string query)
        {
            List<Automobile> obj = new List<Automobile>();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj.Add(GetAutomobile(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }
        public List<Automobile> ToList() => Select("SELECT * FROM automobiles");
    }
    public class TTNSTable : IDBSetBase<TTN>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS ttns " +
                "(ID int NOT NULL AUTO_INCREMENT, automobileid int not null, driver varchar(300) not null, " +
            "contractid int not null, pogruzka varchar(100) not null, dat date null, sdalEmployee int not null, " +
            "responseEmployee int not null," +
            "adrpogruzka varchar(300) not null, adrrazgruzka varchar(300) not null, PRIMARY KEY (ID));";

        public TTNSTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }
        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM ttns;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }
        private TTN GetTTN(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            Automobile auto = App.DbContext.Automobiles.ElementAt(reader.GetInt32(1));
            string empl = reader.GetString(2);
            Contract contract = App.DbContext.Contracts.ElementAt(reader.GetInt32(3));
            string pogruzka = reader.GetString(4);
            DateTime date = reader.IsDBNull(5) ? new DateTime(0, 0, 0) : reader.GetDateTime(5);
            Employee sdal = App.DbContext.Employees.ElementAt(reader.GetInt32(6));
            Employee response = App.DbContext.Employees.ElementAt(reader.GetInt32(7));
            string pogr = reader.GetString(8);
            string razgr = reader.GetString(9);

            return new TTN(id, contract, empl, auto, date, pogruzka, sdal, response, pogr, razgr);
        }
        public TTN ElementAt(int id)
        {
            TTN obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM ttns WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetTTN(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }
        public void Update(TTN obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("UPDATE ttns SET automobileid = @a, driver = @d, contractid = @c, pogruzka = @p, " +
                "dat = @dt, sdalEmployee = @sd, responseEmploye = @rs, adrrazgruzka = @raz, adrpogruzka = @pog WHERE ID = @i;", _connection))
            {
                command.Parameters.AddWithValue("@a", obj.Automobile!.ID);
                command.Parameters.AddWithValue("@d", obj.Driver);
                command.Parameters.AddWithValue("@c", obj.Contract!.ID);
                command.Parameters.AddWithValue("@p", obj.PogruzkaMethod);
                command.Parameters.AddWithValue("@dt", obj.Date);
                command.Parameters.AddWithValue("@sd", obj.SdalEmployee!.ID);
                command.Parameters.AddWithValue("@rs", obj.ResponseEmployee!.ID);
                command.Parameters.AddWithValue("@raz", obj.AdresRazgruzki);
                command.Parameters.AddWithValue("@pog", obj.AdresPogruzki);
                command.Parameters.AddWithValue("@i", obj.ID);
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
        public void Add(TTN obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO ttns " +
                "(automobileid,driver,contractid,pogruzka,dat,sdalEmployee, responseEmployee, adrpogruzka, adrrazgruzka) VALUES " +
                "(@a,@d,@c,@p,@dt,@sd, @rs, @raz, @pog);", _connection))
            {
                command.Parameters.AddWithValue("@a", obj.Automobile!.ID);
                command.Parameters.AddWithValue("@d", obj.Driver);
                command.Parameters.AddWithValue("@c", obj.Contract!.ID);
                command.Parameters.AddWithValue("@p", obj.PogruzkaMethod);
                command.Parameters.AddWithValue("@dt", obj.Date);
                command.Parameters.AddWithValue("@sd", obj.SdalEmployee!.ID);
                command.Parameters.AddWithValue("@rs", obj.ResponseEmployee!.ID);
                command.Parameters.AddWithValue("@raz", obj.AdresRazgruzki);
                command.Parameters.AddWithValue("@pog", obj.AdresPogruzki);
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }
        public void Remove(TTN obj) => Remove(obj.ID);
        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM ttns WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }
        public List<TTN> ToList() => Select("SELECT * FROM ttns;");
        public List<TTN> Select(string query)
        {
            List<TTN> ttns = new List<TTN>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetTTN(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return ttns;
        }
    }
    public class AccountsTable : IDBSetBase<Account>
    {
        private readonly MySqlConnection _connection;
        public const string CreateQuery = 
            "CREATE TABLE IF NOT EXISTS accounts (ID int NOT NULL AUTO_INCREMENT, sellerid int not null, buyerid int not null, contractid int not null, date date not null, PRIMARY KEY (ID));";

        public AccountsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM accounts;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Account GetAccount(MySqlDataReader reader)
        {
            var sel = App.DbContext.Organizations.ElementAt(reader.GetInt32(1));
            var buy = App.DbContext.Organizations.ElementAt(reader.GetInt32(2));
            var contract = App.DbContext.Contracts.ElementAt(reader.GetInt32(3));
            return new Account(reader.GetInt32(0),reader.GetDateTime(4),sel,buy,contract);
        }

        public Account ElementAt(int id)
        {
            Account obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM accounts WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetAccount(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }

        public void Update(Account obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand(
                $"UPDATE accounts SET Sellerid = {obj.Seller!.ID}, buyerid = {obj.Buyer!.ID}, contractid = {obj.Contract!.ID}, date = '{obj.Date.Value.ToMySQLDate()}' WHERE ID = {obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Add(Account obj)
        {
            using (MySqlCommand command = new MySqlCommand($"INSERT INTO accounts (Sellerid, buyerid, contractid, date) values ({obj.Seller!.ID},{obj.Buyer!.ID},{obj.Contract!.ID},'{obj.Date?.ToMySQLDate()}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQuery();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Account obj) => Remove(obj.ID);

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM accounts WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }


        public List<Account> ToList() => Select("SELECT * FROM accounts;");

        public List<Account> Select(string query)
        {
            List<Account> ttns = new List<Account>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetAccount(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return ttns;
        }
    }
    public class ContractMaterialsTable
    {
        public const string CreateQuery = "CREATE TABLE contractMaterials" +
            "(id int not null auto_increment, contractid int not null, materialid int not null, count float null, primary key (id));";

        private ContractMaterial GetContractMaterial(MySqlDataReader reader) =>
            new ContractMaterial(reader.GetInt32(0), reader.GetInt32(2), reader.IsDBNull(3) ? 0 : reader.GetFloat(3));

        public List<ContractMaterial> ToList(int contractId)
        {
            List<ContractMaterial> list = new List<ContractMaterial>(32);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"select * from contractMaterials where contractid = {contractId};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        list.Add(GetContractMaterial(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return list;
        }

        public void Add(int contractid, int materialid, float count)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"INSERT INTO contractMaterials (contractid, materialid, count) VALUES ({contractid},{materialid},{count});", _connection))
                {
                    command.ExecuteNonQuery();
                }
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(int id)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"DELETE FROM contractMaterials where id = {id};", _connection))
                {
                    command.ExecuteNonQuery();
                }
                _connection.CloseAsync().Wait();
            }
        }
        public void Update(ContractMaterial obj)
        {
            using (var _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"UPDATE contractMaterials SET contractid = {obj.ContactID}, " +
                    $"materialid = {obj.Material!.ID}," +
                    $"count = '{obj.Count}' WHERE ID = {obj.ID};", _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }

        public ContractMaterial? ElementAt(int id)
        {
            ContractMaterial? mat = null;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM contractmaterials where id = {id};", _connection))
                {
                    var reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        mat = GetContractMaterial(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return mat;
        }
    }
    public class ContractsTable : IDBSetBase<Contract>
    {
        private readonly MySqlConnection _connection;
        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS contracts " +
            "(id int not null auto_increment, seller int null, buyer int null," +
            "date date, logisticstype varchar(100),individualid int null, PRIMARY KEY (ID));";

        public ContractsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM contracts;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Contract GetContract(MySqlDataReader reader)
        {
            var seller = App.DbContext.Organizations.ElementAt(reader.GetInt32(1));
            var buyer = reader.IsDBNull(2) ? new Organization() : App.DbContext.Organizations.ElementAt(reader.GetInt32(2));
            var mats = App.DbContext.ContractMaterials.ToList(reader.GetInt32(0));
            var log = reader.GetString(4);
            var indiv = reader.IsDBNull(5) ? new Individual() : App.DbContext.Individuals.ElementAt(reader.GetInt32(5));
            return new Contract(reader.GetInt32(0), seller, buyer, reader.GetDateTime(3), mats, log, indiv);
        }
        
        public Contract ElementAt(int id)
        {
            Contract obj = null!;
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM contracts WHERE id={id};", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        obj = GetContract(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return obj;
        }

        public void Update(Contract obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE contracts SET Seller = {obj.Seller!.ID}, {(obj.Buyer != null ? $"Buyer = {obj.Buyer.ID}," : "")}" +
                $"Date = '{obj.Date?.ToMySQLDate()}' logisticstype = '{obj.LogisiticsType}' {(obj.Individual != null ? $", individualid = {obj.Individual.ID}" : "")} WHERE ID = {obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
                foreach (var item in obj.Materials)
                {
                    if (item.ID > 0)
                    {
                        App.DbContext.ContractMaterials.Update(item);
                    }
                    else
                    {
                        item.ContactID = obj.ID;
                        App.DbContext.ContractMaterials.Add(item.ContactID, item.Material!.ID, (float)(item.Count!));
                    }
                }
            }
            _connection.CloseAsync().Wait();
        }

        public void Add(Contract obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO contracts " +
                $"(Seller, {(obj.Buyer != null ? "Buyer," : "")} Date,logisticstype{(obj.Individual != null ? ",individualid" : string.Empty)}) VALUES" +
                $"({obj.Seller!.ID},{(obj.Buyer != null ? obj.Buyer.ID + "," : "")} '{obj.Date?.ToMySQLDate()}', '{obj.LogisiticsType}'{(obj.Individual != null ? "," + obj.Individual.ID : "")});", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQuery();
                _connection.CloseAsync().Wait();
            }

            int contrId = 0;

            using (MySqlCommand command = new MySqlCommand("SELECT ID FROM CONTRACTS WHERE ID = (SELECT MAX(ID) FROM CONTRACTS);", _connection))
            {
                _connection.OpenAsync().Wait();
                var reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    contrId = reader.GetInt32(0);
                }
                _connection.CloseAsync().Wait();
            }

            if (obj.Materials.Count > 0)
            {
                foreach (var item in obj.Materials)
                {
                    item.ContactID = contrId;
                    App.DbContext.ContractMaterials.Add(item.ContactID, item.Material!.ID, (float)(item.Count!));
                }
            }

        }

        public void Remove(Contract obj) => Remove(obj.ID);

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM contracts WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Contract> ToList() => Select("SELECT * FROM contracts;");

        public List<Contract> Select(string query)
        {
            List<Contract> ttns = new List<Contract>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetContract(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return ttns;
        }
    }
    public class PayTypesTable : IDBSetBase<PayType>
    {
        public const string CreateQuery = "CREATE TABLE PAYTYPES (ID INT NOT NULL auto_increment, NAME VARCHAR(100) NOT NULL, PRIMARY KEY(ID));";

        private readonly MySqlConnection connection;

        public PayTypesTable()
        {
            connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public void Add(PayType item)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"INSERT INTO PAYTYPES (NAME) VALUES ('{item.Name}');", _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }

        public void Update(PayType item)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"update PAYTYPES set name = '{item.Name}' where id = {item.ID};", _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(PayType item)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"delete from paytypes where id = {item.ID};", _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }

        public List<PayType> ToList() => Select("SELECT * FROM PAYTYPES;");

        public List<PayType> Select(string query)
        {
            List<PayType> ttns = new List<PayType>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        ttns.Add(GetPayType(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return ttns;
        }

        private PayType GetPayType(MySqlDataReader reader)
        {
            int id = (int)reader[0];
            string name = (string)reader[1];

            return new PayType(id, name);
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
    public class IndividualsTable : IDBSetBase<Individual>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS individuals" +
                "(ID int NOT NULL AUTO_INCREMENT, Name varchar(50), Surname varchar(50)," +
                "Pathnetic varchar(70), PhoneNumber varchar(14), passportid int not null, PRIMARY KEY (ID));";

        public IndividualsTable()
        {
            _connection = new MySqlConnection(StaticValues.ConnectionString);
        }

        public int Count()
        {
            using (MySqlCommand _command = new MySqlCommand("SELECT COUNT(ID) FROM individual;", _connection))
            {
                _connection.OpenAsync().Wait();
                int readedCount = 0;
                using (MySqlDataReader reader = _command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        readedCount = reader.GetInt32(0);
                    }
                _connection.CloseAsync().Wait();
                return readedCount;
            }
        }

        private Individual GetIndividual(MySqlDataReader reader)
        {
            int id = (int)reader[0];
            string name = (string)reader[1];
            string surname = (string)reader[2];
            string pathnetic = (string)reader[3];
            string phonenumber = (string)reader[4];
            int pasid = (int)reader[5];
            Passport passport = App.DbContext.Passports.ElementAt(pasid);
            return new Individual(id, name, surname, pathnetic, phonenumber, passport);
        }

        public Individual ElementAt(int id)
        {
            Individual item = new Individual();
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"SELECT * FROM individuals WHERE id={id};", _connection))
            {
                using (MySqlDataReader reader = command.ExecuteMySqlReaderAsync())
                    while (reader.Read())
                    {
                        item = GetIndividual(reader);
                    }
            }
            _connection.CloseAsync().Wait();
            return item;
        }

        public void Update(Individual obj)
        {
            App.DbContext.Passports.Update(obj.Passport);
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"UPDATE individuals SET Name = '{obj.Name}', Surname = '{obj.Surname}'," +
                $"pathnetic = '{obj.Pathnetic}', phonenumber = '{obj.PhoneNumber}'," +
                $"passportid = {obj.Passport.ID} WHERE ID = {obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Add(Individual obj)
        {
            App.DbContext.Passports.Add(obj.Passport);
            int id = 0;
            using (MySqlCommand command = new MySqlCommand(
                $"SELECT ID FROM PASSPORTS WHERE NUMBER = '{obj.Passport.Number}'", _connection))
            {
                _connection.OpenAsync().Wait();
                var reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    id = (int)reader[0];
                }
                _connection.CloseAsync().Wait();
            }
            using (MySqlCommand command = new MySqlCommand("INSERT INTO individuals " +
                "(Name, Surname, pathnetic, phonenumber, passportid) VALUES" +
                $"('{obj.Name}','{obj.Surname}'," +
                $"'{obj.Pathnetic}','{obj.PhoneNumber}'," +
                $"{id});", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Individual obj) => Remove(obj.ID);

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM individuals WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Individual> Search(string text)
        {
            List<Individual> employees = new List<Individual>(64);
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"SELECT * FROM individuals WHERE " +
                $"CONCAT(name,' ', surname,' ', pathnetic,' ',phonenumber) like '%{text}%';", _connection))
            {
                MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    employees.Add(GetIndividual(reader));
                }
            }
            _connection.CloseAsync().Wait();
            return employees;
        }

        public List<Individual> ToList() => Select("SELECT * FROM individuals;");

        public List<Individual> Select(string query)
        {
            List<Individual> employees = new List<Individual>(64);
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, _connection))
            {
                MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                while (reader.Read())
                {
                    employees.Add(GetIndividual(reader));
                }
            }
            _connection.CloseAsync().Wait();
            return employees;
        }
    }
    public class PassportsTable : IDBSetBase<Passport>
    {
        public const string CreateQuery = "CREATE TABLE passports " +
            "(ID INT NOT NULL auto_increment, number VARCHAR(300) NOT NULL, " +
            "issuedate varchar(300), issuepunkt varchar(300), PRIMARY KEY(ID));";

        public void Add(Passport item)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"INSERT INTO passports " +
                    $"(number,issuedate,issuepunkt) VALUES ('{item.Number.EncryptText()}','{item.IssueDate?.ToShortDateString().EncryptText()}','{item.IssuePunkt.EncryptText()}');", _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }

        public void Update(Passport item)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"update passports set number = '{item.Number.EncryptText()}', issuedate = '{item.IssueDate?.ToShortDateString().EncryptText()}', issuepunkt = '{item.IssuePunkt.EncryptText()}' where id = {item.ID};", _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Passport item) => Remove(item.ID);

        public List<Passport> ToList() => Select("SELECT * FROM passports;");

        public List<Passport> Select(string query)
        {
            List<Passport> list = new List<Passport>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand(query, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        list.Add(GetPassport(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return list;
        }

        private Passport GetPassport(MySqlDataReader reader)
        {
            int id = (int)reader[0];
            string num = ((string)reader[1]).DecryptText();
            string dec = ((string)reader[2]).DecryptText();
            DateTime isdat = Convert.ToDateTime(dec);
            string punkt = ((string)reader[3]).DecryptText();

            return new Passport(id, num, isdat, punkt);
        }

        public Passport ElementAt(int id)
        {
            Passport passport = new Passport();
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM passports where id = " + id, _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        passport = GetPassport(reader);
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return passport;
        }

        public Passport ElementAt(int id, MySqlConnection _connection)
        {
            Passport passport = new Passport();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM passports where id = " + id, _connection))
            {
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    passport = GetPassport(reader);
                }
            }
            return passport;
        }

        public void Remove(int id)
        {
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"delete from passports where id = {id};", _connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }
                _connection.CloseAsync().Wait();
            }
        }
    }
}