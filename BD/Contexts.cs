using BuildMaterials.Models;
using BuildMaterials.ViewModels;

namespace BuildMaterials.BD
{
    public static class StaticValues
    {
        public const string ConnectionString = "server=localhost;user=root;database=buildmaterials;password=546909023Var;";
        public const string CreateDatabaseConnectionString = "server=localhost;user=root;password=546909023Var;";
    }
    public interface IDBSetBase<T> where T : class
    {
        void Add(T obj);
        void Remove(T obj);
        void Remove(int id);
        public List<T> Select(string query);
    }
    public static class Extension
    {
        public static string ToMySQLDate(this DateTime obj) => $"{obj.Date.Year}-{obj.Date.Month}-{obj.Date.Day}";
    }

    public class ApplicationContext
    {
        public MaterialsTable Materials { get; set; } = null!;
        public EmployeesTable Employees { get; set; } = null!;
        public SellersTable Sellers { get; set; } = null!;
        public TradesTable Trades { get; set; } = null!;
        public TTNSTable TTNs { get; set; } = null!;
        public AccountsTable Accounts { get; set; } = null!;
        public ContractsTable Contracts { get; set; } = null!;
        public MaterialResponsesTable MaterialResponse { get; set; } = null!;
        public PayTypesTable PayTypes { get; set; } = null!;

        public readonly string[] AccessLevel = new string[4] { "Минимальный", "Низкий", "Средний", "Максимальный" };

        private bool CheckBDCreated()
        {
            bool res = false;
            using (MySqlConnection con = new MySqlConnection(StaticValues.CreateDatabaseConnectionString))
            {
                con.Open();
                MySqlCommand comm = new MySqlCommand("SELECT * FROM information_schema.tables WHERE table_schema = 'buildmaterials' AND TABLE_NAME = 'materials' LIMIT 1", con);
                MySqlDataReader reader = comm.ExecuteReader();
                res = reader.HasRows;
                con.Close();
            }
            return res;
        }

        public ApplicationContext()
        {
            PayTypes = new PayTypesTable();
            Employees = new EmployeesTable();
            Sellers = new SellersTable();
            Materials = new MaterialsTable();
            Trades = new TradesTable();
            TTNs = new TTNSTable();
            Accounts = new AccountsTable();
            Contracts = new ContractsTable();
            MaterialResponse = new MaterialResponsesTable();
            CreateDatabase();
        }

        public void CreateDatabase()
        {
            if (!CheckBDCreated())
            {
                InitializeDatabase();
                Employees.Add(new Employee(-1, "Имя", "Фамилия", "Отчество", "Администратор", "+375259991234", "BM3132131", DateTime.Now));
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
            Query(EmployeesTable.CreateQuery + SellersTable.CreateQuery + MaterialsTable.CreateQuery +
                TradesTable.CreateQuery + TTNSTable.CreateQuery + AccountsTable.CreateQuery + ContractsTable.CreateQuery
                + MaterialResponsesTable.CreateQuery + PayTypesTable.CreateQuery);
            CreateFKs();
        }

        private void CreateFKs()
        {
            string FKsQuery = "ALTER TABLE TRADES ADD FOREIGN KEY (MATERIALID) REFERENCES MATERIALS(ID) ON DELETE SET NULL;" +
                "ALTER TABLE TRADES ADD FOREIGN KEY (SellerId) REFERENCES EMPLOYEES(ID) ON DELETE SET NULL;" +
                "ALTER TABLE TRADES ADD FOREIGN KEY (paytypeid) REFERENCES paytypes(ID) ON DELETE SET NULL;" +
                "ALTER TABLE materialresponses add foreign key (MaterialID) references materials(id) on delete set null;" +
                "ALTER TABLE materialresponses add foreign key (FinResponseEmployeeID) references EMPLOYEES(id) on delete set null;" +
                "ALTER TABLE ttns add foreign key (materialid) references materials(id) on delete set null;" +
                "ALTER TABLE accounts add foreign key (materialid) references materials(id) on delete set null;" +
                "ALTER TABLE accounts add foreign key (seller) references sellers(id) on delete set null;" +
                "ALTER TABLE accounts add foreign key (Buyer) references sellers(id) on delete set null;" +
                "ALTER TABLE contracts add foreign key (Buyer) references sellers(id) on delete set null;" +
                "ALTER TABLE contracts add foreign key (seller) references sellers(id) on delete set null;" +
                "ALTER TABLE contracts add foreign key (MaterialID) references materials(id) on delete set null;";
            Query(FKsQuery);
        }

        private void InitDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(StaticValues.CreateDatabaseConnectionString))
            {
                connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS buildmaterials;", connection))
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
    public class MaterialsTable : IDBSetBase<Material>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS materials" +
                "(ID int NOT NULL AUTO_INCREMENT, Name varchar(300) not null," +
                "Manufacturer varchar(100) not null, Price float NOT NULL," +
                "Count float NOT NULL,CountUnits varchar(20)," +
                "EnterDate datetime NOT NULL, PRIMARY KEY (ID));";

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
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand("INSERT INTO materials " +
                "(Name, Manufacturer, Price, Count, CountUnits, EnterDate) VALUES" +
                $"('{obj.Name}','{obj.Manufacturer}',{obj.Price},{obj.Count},'{obj.CountUnits}','{obj.EnterDate.Year}-{obj.EnterDate.Month}-{obj.EnterDate.Day}');", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public void Remove(Material obj)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM materials WHERE id={obj.ID};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
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
            return Select($"SELECT * FROM Materials WHERE CONCAT(name,' ', manufacturer,' ', price,' ',count,' ',enterdate) like '%{text}%';");
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
            return new Material(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetFloat(3), reader.GetFloat(4), reader.GetString(5), reader.GetDateTime(6));
        }

        public List<Material> ToList()
        {
            return Select("SELECT * FROM Materials;");
        }
    }
    public class EmployeesTable : IDBSetBase<Employee>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS employees" +
                "(ID int NOT NULL AUTO_INCREMENT, Name varchar(50), Surname varchar(50)," +
                "Pathnetic varchar(70), Position varchar(100), PhoneNumber varchar(14)," +
                "Password int NOT NULL, AccessLevel int NOT NULL, FinResponsible boolean,PassportNumber varchar(9)," +
                "PassportIssueDate date, PRIMARY KEY (ID));";

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
            return new Employee(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                reader.GetString(5), reader.GetString(9), reader.GetDateTime(10), reader.GetInt32(6), reader.GetInt32(7), reader.GetBoolean(8));
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

        public void Add(Employee obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO employees " +
                "(Name, Surname, pathnetic, position, phonenumber, password, AccessLevel, FinResponsible,PassportIssueDate,PassportNumber) VALUES" +
                $"('{obj.Name}','{obj.SurName}'," +
                $"'{obj.Pathnetic}','{obj.Position}','{obj.PhoneNumber}',{obj.Password},{obj.AccessLevel},{obj.FinResponsible}," +
                $"'{obj.PassportIssueDate?.ToMySQLDate()}','{obj.PassportNumber}');", _connection))
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

        public List<Employee> ToList()
        {
            return Select("SELECT * FROM employees;");
        }

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
    public class SellersTable : IDBSetBase<Organization>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS sellers " +
                "(ID int NOT NULL AUTO_INCREMENT, CompanyName varchar(200), Adress varchar(50)," +
                "ShortCompanyName varchar(70), RegistrationDate date, mnsnumber varchar(5)," +
                "mnsname varchar(100) NOT NULL, UNP varchar(100) NOT NULL, PRIMARY KEY (ID));";

        public SellersTable()
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

            return new Organization(id, compName, shortcmpname, address,regdate,mnsnum,mnsname, unp);
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

        public void Add(Organization obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO Sellers " +
                "(companyname, adress, ShortCompanyName, RegistrationDate, mnsnumber, mnsname, UNP) VALUES" +
                $"('{obj.CompanyName}','{obj.Adress}'," +
                $"'{obj.ShortCompamyName}','{obj.RegistrationDate.Value.ToMySQLDate()}','{obj.MNSNumber}','{obj.MNSName}','{obj.UNP}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
        }

        public void Remove(Organization obj)
        {
            Remove(obj.ID);
        }

        public void Remove(int id)
        {
            _connection.OpenAsync().Wait();
            using (MySqlCommand command = new MySqlCommand($"DELETE FROM Sellers WHERE id={id};", _connection))
            {
                command.ExecuteNonQueryAsync().Wait();
            }
            _connection.CloseAsync().Wait();
        }

        public List<Organization> Search(string text, bool iscustomer = false) =>
            Select($"SELECT * FROM Sellers WHERE CONCAT(companyname,' ', adress,' ', companyperson,' ',bank,' ',bankprop,' ', unp) like '%{text}%' and iscustomer = {iscustomer};");

        public List<Organization> ToList(bool iscustomer = false)
        {
            return Select($"SELECT * FROM sellers where iscustomer = {iscustomer};");
        }

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
                "(ID int NOT NULL AUTO_INCREMENT, Date date not null, SellerId int NULL," +
                "MaterialID int null, count float, price float,paytypeid int, PRIMARY KEY (ID));";

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
            int? materialid = reader.IsDBNull(3) ? null : (int)reader["materialid"];
            float? count = reader.IsDBNull(4) ? null : (float)reader["count"];
            float? price = reader.IsDBNull(5) ? null : (float)reader["price"];
            int paytypeid = (int)reader[6];
            return new Trade(id, date, sellerid, materialid, count, price, paytypeid);
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
                $"('{obj.Date.ToMySQLDate()}',{sellerValue}" +
                $"{obj.MaterialID},{obj.Count},{obj.Price},{obj.PayTypeID});", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
            }
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

        public void Update(Trade obj) => App.DbContext.Query($"UPDATE Trades SET Count = {obj.Count}, Date = '{obj.Date.ToMySQLDate()}'," +
            $"MaterialID = {obj.MaterialID},PayTypeID = {obj.PayTypeID},Price = {obj.Price},SellerID = {obj.SellerID},Summ = {obj.Summ} " +
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
    public class TTNSTable : IDBSetBase<TTN>
    {
        private readonly MySqlConnection _connection;

        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS ttns " +
                "(ID int NOT NULL AUTO_INCREMENT, Shipper varchar(100), " +
                "Consignee varchar(100) null," +
                "Payer varchar(100) not null, count float not null," +
                "price float, materialid int null," +
                "CountUnits varchar(20), weight float not null," +
                "date date null, PRIMARY KEY (ID));";

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
            string? shiper = reader.IsDBNull(1) ? null : (string)reader[1];
            string? consignee = reader.IsDBNull(2) ? null : (string)reader[2];
            string payer = (string)reader[3];
            float count = (float)reader[4];
            float? price = reader.IsDBNull(5) ? null : (float)reader[5];
            int? matid = reader.IsDBNull(6) ? null : (int)reader[6];
            string? units = reader.IsDBNull(7) ? null : (string)reader[7];
            float weight = (float)reader[8];
            DateTime date = (DateTime)reader[9];
            return new TTN(id, shiper, consignee, payer, count, price, matid, units, weight, date);
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

        public void Add(TTN obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO ttns " +
                "(shipper, Consignee, Payer, Count, Price,Weight, Date, materialid,CountUnits) VALUES" +
                $"('{obj.Shipper}','{obj.Consignee}'," +
                $"'{obj.Payer}',{obj.Count},{obj.Price},{obj.Weight},'{obj.Date?.ToMySQLDate()}'," +
                $"'{obj.MaterialID}','{obj.CountUnits}');", _connection))
            {
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

        public List<TTN> Search(string text)
        {
            List<TTN> ttns = new List<TTN>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM ttns" +
                    $" WHERE CONCAT(Shipper,' ', Consignee,' ',Payer,' ',MaterialName) like '%{text}%';)", _connection))
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
        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS accounts  (ID int NOT NULL AUTO_INCREMENT, Seller int, ShipperName varchar(50) not null," +
                "ShipperAdress varchar(100) not null, ConsigneeName varchar(50) not null," +
                "ConsigneeAdress varchar(100) not null, Buyer int null," +
                "CountUnits varchar(20), Count float not null," +
                "Price float not null, Tax float not null, date date not null," +
                "materialid int null, PRIMARY KEY (ID));";

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
            return new Account(reader.GetInt32(0), reader.IsDBNull(1) ? -1 : reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                reader.GetString(5), reader.IsDBNull(6) ? -1 : reader.GetInt32(6), reader.GetString(7),
                reader.GetFloat(8), reader.GetFloat(9), reader.GetFloat(10), reader.GetDateTime(11), reader.GetInt32(12));
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

        public void Add(Account obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO accounts " +
                "(Seller, ShipperName, ShipperAdress, ConsigneeName, ConsigneeAdress," +
                "Buyer,CountUnits, Count,Price,Tax,Date, materialid) VALUES" +
                $"({obj.SellerID},'{obj.ShipperName}','{obj.ShipperAdress}','{obj.ConsigneeName}'," +
                $"'{obj.ConsigneeAdress}',{obj.BuyerID},'{obj.CountUnits}',{obj.Count},{obj.Price},{obj.Tax}," +
                $"'{obj.Date?.ToMySQLDate()}',{obj.MaterialID});", _connection))
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

        public List<Account> Search(string text)
        {
            List<Account> ttns = new List<Account>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM accounts" +
                    $" WHERE CONCAT(Seller,' ', ShipperName,' ',ShipperAdress,' ',ConsigneeName," +
                    $"' ',ConsigneeAdress,' ',Buyer) like '%{text}%';)", _connection))
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
    public class ContractsTable : IDBSetBase<Contract>
    {
        private readonly MySqlConnection _connection;
        public const string CreateQuery = "CREATE TABLE IF NOT EXISTS contracts " +
                "(id int not null auto_increment, seller int null, buyer int null," +
                "MaterialID int null, count float not null," +
                "countunits varchar(30), price float, date date, PRIMARY KEY (ID));";
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
            return new Contract(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetFloat(4),
                reader.GetString(5), reader.GetFloat(6), reader.GetDateTime(7));
        }

        public BuildMaterials.Models.Contract ElementAt(int id)
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

        public void Add(Contract obj)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO contracts " +
                "(Seller, Buyer, MaterialID, Count, CountUnits," +
                "Price, Date) VALUES" +
                $"('{obj.SellerID}','{obj.BuyerID}','{obj.MaterialID}',{obj.Count}," +
                $"'{obj.CountUnits}',{obj.Price},'{obj.Date?.ToMySQLDate()}');", _connection))
            {
                _connection.OpenAsync().Wait();
                command.ExecuteNonQueryAsync().Wait();
                _connection.CloseAsync().Wait();
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

        public List<Contract> Search(string text)
        {
            List<Contract> contracts = new List<Contract>(64);
            using (MySqlConnection _connection = new MySqlConnection(StaticValues.ConnectionString))
            {
                _connection.OpenAsync().Wait();
                using (MySqlCommand command = new MySqlCommand($"SELECT * FROM contracts" +
                    $" WHERE CONCAT(Seller,' ', Buyer,' ',(SELECT Name FROM Materials WHERE ID = MaterialID),' ',ConsigneeName," +
                    $"' ',ConsigneeAdress,' ',Buyer) like '%{text}%';)", _connection))
                {
                    MySqlDataReader reader = command.ExecuteMySqlReaderAsync();
                    while (reader.Read())
                    {
                        contracts.Add(GetContract(reader));
                    }
                }
                _connection.CloseAsync().Wait();
            }
            return contracts;
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
    public class PayTypesTable
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
    }
}